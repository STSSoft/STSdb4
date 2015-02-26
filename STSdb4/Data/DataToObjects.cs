using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using STSdb4.General.Extensions;

namespace STSdb4.Data
{
    public class DataToObjects : IToObjects<IData>
    {
        public readonly Func<IData, object[]> to;
        public readonly Func<object[], IData> from;

        public readonly Type Type;
        public readonly Func<Type, MemberInfo, int> MembersOrder;

        public DataToObjects(Type type, Func<Type, MemberInfo, int> membersOrder = null)
        {
            if (!DataType.IsPrimitiveType(type) && !type.HasDefaultConstructor())
                throw new NotSupportedException("No default constructor.");

            bool isSupported = DataTypeUtils.IsAllPrimitive(type);
            if (!isSupported)
                throw new NotSupportedException("Not all types are primitive.");

            Type = type;
            MembersOrder = membersOrder;

            to = CreateToMethod().Compile();
            from = CreateFromMethod().Compile();
        }

        public Expression<Func<IData, object[]>> CreateToMethod()
        {
            var data = Expression.Parameter(typeof(IData), "data");

            var d = Expression.Variable(typeof(Data<>).MakeGenericType(Type), "d");
            var body = Expression.Block(new ParameterExpression[] { d }, Expression.Assign(d, Expression.Convert(data, d.Type)), ValueToObjectsHelper.ToObjects(d.Value(), MembersOrder));

            return Expression.Lambda<Func<IData, object[]>>(body, data);
        }

        public Expression<Func<object[], IData>> CreateFromMethod()
        {
            var objectArray = Expression.Parameter(typeof(object[]), "item");
            var data = Expression.Variable(typeof(Data<>).MakeGenericType(Type));

            List<Expression> list = new List<Expression>();
            list.Add(Expression.Assign(data, Expression.New(data.Type.GetConstructor(new Type[] { }))));

            if (!DataType.IsPrimitiveType(Type))
                list.Add(Expression.Assign(data.Value(), Expression.New(data.Value().Type.GetConstructor(new Type[] { }))));

            list.Add(ValueToObjectsHelper.FromObjects(data.Value(), objectArray, MembersOrder));
            list.Add(Expression.Label(Expression.Label(typeof(IData)), data));

            var body = Expression.Block(typeof(IData), new ParameterExpression[] { data }, list);

            return Expression.Lambda<Func<object[], IData>>(body, objectArray);
        }

        public object[] To(IData value1)
        {
            return to(value1);
        }

        public IData From(object[] value2)
        {
            return from(value2);
        }
    }
}

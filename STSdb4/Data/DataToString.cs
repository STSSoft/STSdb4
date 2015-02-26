using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using STSdb4.General.Extensions;
using System.Reflection;

namespace STSdb4.Data
{
    public class DataToString : IToString<IData>
    {
        public readonly Func<IData, string> to;
        public readonly Func<string, IData> from;

        public readonly Type Type;
        public readonly int StringBuilderCapacity;
        public readonly IFormatProvider[] Providers;
        public readonly char[] Delimiters;
        public readonly Func<Type, MemberInfo, int> MembersOrder;

        public DataToString(Type type, int stringBuilderCapacity, IFormatProvider[] providers, char[] delimiters, Func<Type, MemberInfo, int> membersOrder = null)
        {
            Type = type;
            StringBuilderCapacity = stringBuilderCapacity;
            var typeCount = DataType.IsPrimitiveType(type) ? 1 : DataTypeUtils.GetPublicMembers(type, membersOrder).Count();
            if (providers.Length != typeCount)
                throw new ArgumentException("providers.Length != dataType.Length");

            Providers = providers;
            Delimiters = delimiters;
            MembersOrder = membersOrder;

            to = CreateToMethod().Compile();
            from = CreateFromMethod().Compile();
        }

        public DataToString(Type type, int stringBuilderCapacity, char[] delimiters, Func<Type, MemberInfo, int> membersOrder = null)
            : this(type, stringBuilderCapacity, ValueToStringHelper.GetDefaultProviders(type, membersOrder), delimiters, membersOrder)
        {
        }

        public DataToString(Type type, Func<Type, MemberInfo, int> membersOrder = null)
            : this(type, 16, new char[] { ';' }, membersOrder)
        {
        }

        public Expression<Func<IData, string>> CreateToMethod()
        {
            var data = Expression.Parameter(typeof(IData), "data");
            var d = Expression.Variable(typeof(Data<>).MakeGenericType(Type), "d");

            List<Expression> list = new List<Expression>();
            list.Add(Expression.Assign(d, Expression.Convert(data, typeof(Data<>).MakeGenericType(Type))));
            list.Add(ValueToStringHelper.CreateToStringBody(d.Value(), StringBuilderCapacity, Providers, Delimiters[0], MembersOrder));

            var body = Expression.Block(new ParameterExpression[] { d }, list);

            return Expression.Lambda<Func<IData, string>>(body, data);
        }

        public Expression<Func<string, IData>> CreateFromMethod()
        {
            var stringParam = Expression.Parameter(typeof(string), "item");
            List<Expression> list = new List<Expression>();

            var data = Expression.Variable(typeof(Data<>).MakeGenericType(Type), "d");

            list.Add(Expression.Assign(data, Expression.New(data.Type.GetConstructor(new Type[] { }))));

            if (!DataType.IsPrimitiveType(Type))
                list.Add(Expression.Assign(data.Value(), Expression.New(Type.GetConstructor(new Type[] { }))));

            list.Add(ValueToStringHelper.CreateParseBody(data.Value(), stringParam, Providers, Delimiters, MembersOrder));
            list.Add(Expression.Label(Expression.Label(typeof(Data<>).MakeGenericType(Type)), data));

            var body = Expression.Block(new ParameterExpression[] { data }, list);

            return Expression.Lambda<Func<string, IData>>(body, new ParameterExpression[] { stringParam });
        }

        public string To(IData value1)
        {
            return to(value1);
        }

        public IData From(string value2)
        {
            return from(value2);
        }
    }
}

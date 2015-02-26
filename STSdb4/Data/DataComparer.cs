using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace STSdb4.Data
{
    public class DataComparer : IComparer<IData>
    {
        public readonly Func<IData, IData, int> compare;

        public readonly Type Type;
        public readonly Type DataType;
        public readonly CompareOption[] CompareOptions;
        public readonly Func<Type, MemberInfo, int> MembersOrder;

        public DataComparer(Type type, CompareOption[] compareOptions, Func<Type, MemberInfo, int> membersOrder = null)
        {
            Type = type;
            DataType = typeof(Data<>).MakeGenericType(type);

            CompareOption.CheckCompareOptions(type, compareOptions, membersOrder);
            CompareOptions = compareOptions;
            MembersOrder = membersOrder;

            compare = CreateCompareMethod().Compile();
        }

        public DataComparer(Type type, Func<Type, MemberInfo, int> membersOrder = null)
            : this(type, CompareOption.GetDefaultCompareOptions(type, membersOrder), membersOrder)
        {
        }

        public Expression<Func<IData, IData, int>> CreateCompareMethod()
        {
            var x = Expression.Parameter(typeof(IData));
            var y = Expression.Parameter(typeof(IData));

            List<Expression> list = new List<Expression>();
            List<ParameterExpression> parameters = new List<ParameterExpression>();

            var value1 = Expression.Variable(Type, "value1");
            parameters.Add(value1);
            list.Add(Expression.Assign(value1, Expression.Convert(x, DataType).Value()));

            var value2 = Expression.Variable(Type, "value2");
            parameters.Add(value2);
            list.Add(Expression.Assign(value2, Expression.Convert(y, DataType).Value()));

            return Expression.Lambda<Func<IData, IData, int>>(ComparerHelper.CreateComparerBody(list, parameters, value1, value2, CompareOptions, MembersOrder), x, y);
        }

        public int Compare(IData x, IData y)
        {
            return compare(x, y);
        }
    }
}

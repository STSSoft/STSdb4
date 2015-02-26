using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using STSdb4.General.Extensions;
using STSdb4.General.Comparers;

namespace STSdb4.Data
{
    public class DataEqualityComparer : IEqualityComparer<IData>
    {
        public readonly Func<IData, IData, bool> equals;
        public readonly Func<IData, int> getHashCode;

        public readonly Type Type;
        public readonly Func<Type, MemberInfo, int> MembersOrder;
        public readonly CompareOption[] CompareOptions;

        public DataEqualityComparer(Type type, CompareOption[] compareOptions, Func<Type, MemberInfo, int> membersOrder = null)
        {
            Type = type;
            CompareOption.CheckCompareOptions(type, compareOptions, membersOrder);
            CompareOptions = compareOptions;
            MembersOrder = membersOrder;

            equals = CreateEqualsMethod().Compile();
            getHashCode = CreateGetHashCodeMethod().Compile();
        }

        public DataEqualityComparer(Type type, Func<Type, MemberInfo, int> membersOrder = null)
            : this(type, CompareOption.GetDefaultCompareOptions(type, membersOrder), membersOrder)
        {
        }

        public Expression<Func<IData, IData, bool>> CreateEqualsMethod()
        {
            var x = Expression.Parameter(typeof(IData));
            var y = Expression.Parameter(typeof(IData));
            var xValue = Expression.Variable(Type);
            var yValue = Expression.Variable(Type);

            var dataType = typeof(Data<>).MakeGenericType(Type);

            var body = Expression.Block(typeof(bool), new ParameterExpression[] { xValue, yValue },
                    Expression.Assign(xValue, Expression.Convert(x, dataType).Value()),
                    Expression.Assign(yValue, Expression.Convert(y, dataType).Value()),
                    EqualityComparerHelper.CreateEqualsBody(xValue, yValue, CompareOptions, MembersOrder)
                );
            var lambda = Expression.Lambda<Func<IData, IData, bool>>(body, x, y);

            return lambda;
        }

        public Expression<Func<IData, int>> CreateGetHashCodeMethod()
        {
            var obj = Expression.Parameter(typeof(IData));
            var objValue = Expression.Variable(Type);

            var dataType = typeof(Data<>).MakeGenericType(Type);

            var body = Expression.Block(typeof(int), new ParameterExpression[] { objValue },
                Expression.Assign(objValue, Expression.Convert(obj, dataType).Value()),
                EqualityComparerHelper.CreateGetHashCodeBody(objValue, MembersOrder)
                );
            var lambda = Expression.Lambda<Func<IData, int>>(body, obj);

            return lambda;
        }

        public bool Equals(IData x, IData y)
        {
            return equals(x, y);
        }

        public int GetHashCode(IData obj)
        {
            return getHashCode(obj);
        }
    }
}

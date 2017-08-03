﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using STSdb4.General.Extensions;
using System.Reflection;

namespace STSdb4.Data
{
    public class ValueToObjects<T> : IToObjects<T>
    {
        public readonly Func<object[], T> from;
        public readonly Func<T, object[]> to;

        public readonly Type Type;
        public readonly Func<Type, MemberInfo, int> MembersOrder;

        public ValueToObjects(Func<Type, MemberInfo, int> membersOrder = null)
        {
            if (!DataType.IsPrimitiveType(typeof(T)) && !typeof(T).HasDefaultConstructor())
                throw new NotSupportedException("No default constructor.");

            bool isSupported = DataTypeUtils.IsAllPrimitive(typeof(T));
            if (!isSupported)
                throw new NotSupportedException("Not all types are primitive.");

            Type = typeof(T);
            MembersOrder = membersOrder;

            to = CreateToMethod().Compile();
            from = CreateFromMethod().Compile();
        }

        public Expression<Func<T, object[]>> CreateToMethod()
        {
            var item = Expression.Parameter(Type);

            return Expression.Lambda<Func<T, object[]>>(ValueToObjectsHelper.ToObjects(item, MembersOrder), item);
        }

        public Expression<Func<object[], T>> CreateFromMethod()
        {
            var objectArray = Expression.Parameter(typeof(object[]), "item");
            var item = Expression.Variable(Type);
            List<Expression> list = new List<Expression>();

            if (!DataType.IsPrimitiveType(Type))
                list.Add(Expression.Assign(item, Expression.New(item.Type.GetConstructor(new Type[] { }))));

            list.Add(ValueToObjectsHelper.FromObjects(item, objectArray, MembersOrder));
            list.Add(Expression.Label(Expression.Label(typeof(T)), item));

            var body = Expression.Block(typeof(T), new ParameterExpression[] { item }, list);

            return Expression.Lambda<Func<object[], T>>(body, objectArray);
        }

        public object[] To(T value1)
        {
            return to(value1);
        }

        public T From(object[] value2)
        {
            return from(value2);
        }
    }

    public static class ValueToObjectsHelper
    {
        public static Expression ToObjects(Expression item, Func<Type, MemberInfo, int> membersOrder)
        {
            Type[] types = DataType.IsPrimitiveType(item.Type) ? new Type[] { item.Type } : DataTypeUtils.GetPublicMembers(item.Type, membersOrder).Select(x => x.GetPropertyOrFieldType()).ToArray();

            if (types.Length == 1)
                return Expression.NewArrayInit(typeof(object), Expression.Convert(item, typeof(object)));

            Expression[] values = new Expression[types.Length];
            int i = 0;

            foreach (var member in DataTypeUtils.GetPublicMembers(item.Type, membersOrder))
                values[i++] = Expression.Convert(Expression.PropertyOrField(item, member.Name), typeof(object));

            return Expression.NewArrayInit(typeof(object), values);
        }

        public static Expression FromObjects(Expression item, ParameterExpression objectArray, Func<Type, MemberInfo, int> membersOrder)
        {
            Type[] types = DataType.IsPrimitiveType(item.Type) ? new Type[] { item.Type } : DataTypeUtils.GetPublicMembers(item.Type, membersOrder).Select(x => x.GetPropertyOrFieldType()).ToArray();

            if (types.Length == 1)
                return Expression.Assign(item, Expression.Convert(Expression.ArrayAccess(objectArray, Expression.Constant(0, typeof(int))), types[0]));

            List<Expression> list = new List<Expression>();
            int i = 0;
            foreach (var member in DataTypeUtils.GetPublicMembers(item.Type, membersOrder))
                list.Add(Expression.Assign(Expression.PropertyOrField(item, member.Name), Expression.Convert(Expression.ArrayAccess(objectArray, Expression.Constant(i, typeof(int))), types[i++])));

            return Expression.Block(list);
        }
    }
}
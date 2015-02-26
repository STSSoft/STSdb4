using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STSdb4.General.Extensions;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace STSdb4.Data
{
    public class ValueToString<T> : IToString<T>
    {
        public readonly Func<T, string> to;
        public readonly Func<string, T> from;

        public readonly Type Type;
        public readonly int StringBuilderCapacity;
        public readonly IFormatProvider[] Providers;
        public readonly char[] Delimiters;
        public readonly Func<Type, MemberInfo, int> MembersOrder;

        public ValueToString(int stringBuilderCapacity, IFormatProvider[] providers, char[] delimiters, Func<Type, MemberInfo, int> membersOrder = null)
        {
            if (!DataType.IsPrimitiveType(typeof(T)) && !typeof(T).HasDefaultConstructor())
                throw new NotSupportedException("No default constructor.");

            bool isSupported = DataTypeUtils.IsAllPrimitive(typeof(T));
            if (!isSupported)
                throw new NotSupportedException("Not all types are primitive.");

            var countOfType = DataType.IsPrimitiveType(typeof(T)) ? 1 : DataTypeUtils.GetPublicMembers(typeof(T), membersOrder).Count();

            if (providers.Length != countOfType)
                throw new ArgumentException("providers.Length != dataType.Length");

            Type = typeof(T);
            MembersOrder = membersOrder;
            StringBuilderCapacity = stringBuilderCapacity;
            Providers = providers;
            Delimiters = delimiters;

            to = CreateToMethod().Compile();
            from = CreateFromMethod().Compile();
        }

        public ValueToString(int stringBuilderCapacity, char[] delimiters, Func<Type, MemberInfo, int> membersOrder = null)
            : this(stringBuilderCapacity, ValueToStringHelper.GetDefaultProviders(typeof(T), membersOrder), delimiters, membersOrder)
        {
        }

        public ValueToString(Func<Type, MemberInfo, int> membersOrder = null)
            : this(16, new char[] { ';' }, membersOrder)
        {
        }

        public Expression<Func<T, string>> CreateToMethod()
        {
            var item = Expression.Parameter(typeof(T));

            return Expression.Lambda<Func<T, string>>(ValueToStringHelper.CreateToStringBody(item, StringBuilderCapacity, Providers, Delimiters[0], MembersOrder), new ParameterExpression[] { item });
        }

        public Expression<Func<string, T>> CreateFromMethod()
        {
            var stringParam = Expression.Parameter(typeof(string), "item");
            List<Expression> list = new List<Expression>();

            var item = Expression.Variable(Type);

            if (!DataType.IsPrimitiveType(Type))
                list.Add(Expression.Assign(item, Expression.New(Type.GetConstructor(new Type[] { }))));

            list.Add(ValueToStringHelper.CreateParseBody(item, stringParam, Providers, Delimiters, MembersOrder));
            list.Add(Expression.Label(Expression.Label(Type), item));

            var body = Expression.Block(new ParameterExpression[] { item }, list);

            return Expression.Lambda<Func<string, T>>(body, new ParameterExpression[] { stringParam });
        }

        public string To(T value1)
        {
            return to(value1);
        }

        public T From(string value2)
        {
            return from(value2);
        }
    }

    public static class ValueToStringHelper
    {
        public static Expression CreateToStringBody(Expression item, int stringBuilderCapacity, IFormatProvider[] providers, char delimiter, Func<Type, MemberInfo, int> membersOrder)
        {
            var stringBuilder = Expression.Variable(typeof(StringBuilder));

            if (DataType.IsPrimitiveType(item.Type) || DataTypeUtils.GetPublicMembers(item.Type, membersOrder).Count() == 1)
            {
                var member = DataType.IsPrimitiveType(item.Type) ? item : Expression.PropertyOrField(item, DataTypeUtils.GetPublicMembers(item.Type, membersOrder).First().Name);

                MethodCallExpression callToString;

                if (member.Type == typeof(byte[]))
                {
                    var toHexMethod = typeof(ByteArrayExtensions).GetMethod("ToHex", new Type[] { typeof(byte[]) });
                    callToString = Expression.Call(toHexMethod, member);
                }
                else if (member.Type == typeof(TimeSpan))
                {
                    var toStringProvider = member.Type.GetMethod("ToString", new Type[] { typeof(String), typeof(IFormatProvider) });
                    callToString = Expression.Call(member, toStringProvider, Expression.Constant(null, typeof(String)), Expression.Constant(providers[0], typeof(IFormatProvider)));
                }
                else
                {
                    var toStringProvider = member.Type.GetMethod("ToString", new Type[] { typeof(IFormatProvider) });
                    callToString = Expression.Call(member, toStringProvider, Expression.Constant(providers[0], typeof(IFormatProvider)));
                }

                return Expression.Label(Expression.Label(typeof(string)), member.Type == typeof(string) ? (Expression)member : (Expression)callToString);
            }

            List<Expression> list = new List<Expression>();
            list.Add(Expression.Assign(stringBuilder, Expression.New(stringBuilder.Type.GetConstructor(new Type[] { typeof(int) }), Expression.Constant(stringBuilderCapacity))));

            int i = 0;
            foreach (var member in DataTypeUtils.GetPublicMembers(item.Type, membersOrder))
            {
                list.Add(GetAppendCommand(Expression.PropertyOrField(item, member.Name), stringBuilder, providers[i]));

                if (i < DataTypeUtils.GetPublicMembers(item.Type, membersOrder).Count() - 1)
                    list.Add(Expression.Call(stringBuilder, typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(char) }), Expression.Constant(delimiter)));
                i++;
            }

            list.Add(Expression.Label(Expression.Label(typeof(string)), Expression.Call(stringBuilder, typeof(object).GetMethod("ToString"))));

            return Expression.Block(new ParameterExpression[] { stringBuilder }, list);
        }

        private static Expression GetAppendCommand(Expression member, ParameterExpression stringBuilder, IFormatProvider provider)
        {
            MethodCallExpression callToString;

            if (member.Type == typeof(byte[]))
            {
                var toHexMethod = typeof(ByteArrayExtensions).GetMethod("ToHex", new Type[] { typeof(byte[]) });
                callToString = Expression.Call(toHexMethod, member);
            }
            else if (member.Type == typeof(TimeSpan))
            {
                var toStringProvider = member.Type.GetMethod("ToString", new Type[] { typeof(String), typeof(IFormatProvider) });
                callToString = Expression.Call(member, toStringProvider, Expression.Constant(null, typeof(String)), Expression.Constant(provider, typeof(IFormatProvider)));
            }
            else
            {
                var toStringProvider = member.Type.GetMethod("ToString", new Type[] { typeof(IFormatProvider) });
                callToString = Expression.Call(member, toStringProvider, Expression.Constant(provider, typeof(IFormatProvider)));
            }

            var apendMethod = typeof(StringBuilder).GetMethod("Append", new Type[] { typeof(String) });
            var callAppend = Expression.Call(stringBuilder, apendMethod, member.Type == typeof(string) ? (Expression)member : (Expression)callToString);

            return callAppend;
        }

        public static Expression CreateParseBody(Expression item, ParameterExpression stringParam, IFormatProvider[] providers, char[] delimiters, Func<Type, MemberInfo, int> membersOrder)
        {
            var array = Expression.Variable(typeof(string[]), "array");

            if (DataType.IsPrimitiveType(item.Type) || DataTypeUtils.GetPublicMembers(item.Type, membersOrder).Count() == 1)
            {
                var member = DataType.IsPrimitiveType(item.Type) ? item : Expression.PropertyOrField(item, DataTypeUtils.GetPublicMembers(item.Type, membersOrder).First().Name);

                Expression value;

                if (member.Type == typeof(String))
                {
                    value = stringParam;
                }
                else if (member.Type == typeof(byte[]))
                {
                    var hexParse = typeof(StringExtensions).GetMethod("ParseHex", new Type[] { typeof(string) });
                    value = Expression.Call(hexParse, stringParam);
                }
                else if (member.Type == typeof(char))
                {
                    var parseMethod = member.Type.GetMethod("Parse", new Type[] { typeof(string) });
                    value = Expression.Call(parseMethod, stringParam);
                }
                else if (member.Type == typeof(bool))
                {
                    var parseMethod = member.Type.GetMethod("Parse");
                    value = Expression.Call(parseMethod, stringParam);
                }
                else
                {
                    var parseMethod = member.Type.GetMethod("Parse", new Type[] { typeof(string), typeof(IFormatProvider) });
                    value = Expression.Call(parseMethod, stringParam, Expression.Constant(providers[0], typeof(IFormatProvider)));
                }

                return Expression.Assign(member, value);
            }

            List<Expression> list = new List<Expression>();
            list.Add(Expression.Assign(array, Expression.Call(stringParam, typeof(string).GetMethod("Split", new Type[] { typeof(char[]) }), new Expression[] { Expression.Constant(delimiters) })));

            int i = 0;
            foreach (var member in DataTypeUtils.GetPublicMembers(item.Type, membersOrder))
                list.Add(GetParseCommand(Expression.PropertyOrField(item, member.Name), i, array, providers[i++]));

            return Expression.Block(new ParameterExpression[] { array }, list);
        }

        private static Expression GetParseCommand(Expression member, int index, ParameterExpression stringArray, IFormatProvider provider)
        {
            var sValue = Expression.ArrayAccess(stringArray, Expression.Constant(index));
            Expression value;

            if (member.Type == typeof(String))
            {
                value = sValue;
            }
            else if (member.Type == typeof(byte[]))
            {
                var hexParse = typeof(StringExtensions).GetMethod("ParseHex", new Type[] { typeof(string) });
                value = Expression.Call(hexParse, sValue);
            }
            else if (member.Type == typeof(char))
            {
                var parseMethod = member.Type.GetMethod("Parse", new Type[] { typeof(string) });
                value = Expression.Call(parseMethod, sValue);
            }
            else if (member.Type == typeof(bool))
            {
                var parseMethod = member.Type.GetMethod("Parse");
                value = Expression.Call(parseMethod, sValue);
            }
            else
            {
                var parseMethod = member.Type.GetMethod("Parse", new Type[] { typeof(string), typeof(IFormatProvider) });
                value = Expression.Call(parseMethod, sValue, Expression.Constant(provider, typeof(IFormatProvider)));
            }

            return Expression.Assign(member, value);
        }

        public static IFormatProvider[] GetDefaultProviders(Type type, Func<Type, MemberInfo, int> membersOrder = null)
        {
            if (DataType.IsPrimitiveType(type))
                return new IFormatProvider[] { GetDefaultProvider(type) };

            List<IFormatProvider> providers = new List<IFormatProvider>();
            foreach (var member in DataTypeUtils.GetPublicMembers(type, membersOrder))
                providers.Add(GetDefaultProvider(member.GetPropertyOrFieldType()));

            return providers.ToArray();
        }

        public static IFormatProvider GetDefaultProvider(Type type)
        {
            if (!DataType.IsPrimitiveType(type))
                throw new NotSupportedException(type.ToString());

            if (type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(decimal))
            {
                NumberFormatInfo numberFormat = new NumberFormatInfo();
                numberFormat.CurrencyDecimalSeparator = ".";

                return numberFormat;
            }
            else if (type == typeof(DateTime) || type == typeof(TimeSpan))
            {
                DateTimeFormatInfo dateTimeFormat = new DateTimeFormatInfo();
                dateTimeFormat.DateSeparator = "-";
                dateTimeFormat.TimeSeparator = ":";
                dateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
                dateTimeFormat.ShortTimePattern = "HH:mm:ss.fff";
                dateTimeFormat.LongDatePattern = dateTimeFormat.ShortDatePattern;
                dateTimeFormat.LongTimePattern = dateTimeFormat.ShortTimePattern;

                return dateTimeFormat;
            }
            else
                return null;
        }
    }
}

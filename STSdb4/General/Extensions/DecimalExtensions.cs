using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace STSdb4.General.Extensions
{
    public delegate int GetDigitsDelegate(ref Decimal value);

    public class DecimalHelper
    {
        public static readonly DecimalHelper Instance = new DecimalHelper();

        public readonly GetDigitsDelegate GetDigits;
        public readonly Expression<GetDigitsDelegate> GetDigitsLambda;

        public DecimalHelper()
        {
            GetDigitsLambda = STSdb4.General.Environment.RunningOnMono ? CreateGetDigitsMonoMethod() : CreateGetDigitsMethod();
            GetDigits = GetDigitsLambda.Compile();
        }

        private Expression<GetDigitsDelegate> CreateGetDigitsMethod()
        {
            var value = Expression.Parameter(typeof(Decimal).MakeByRefType(), "value");

            var digits = Expression.RightShift(
                Expression.And(Expression.Field(value, "flags"), Expression.Constant(~Int32.MinValue, typeof(int))),
                Expression.Constant(16, typeof(int)));

            //return (value.flags & ~Int32.MinValue) >> 16

            return Expression.Lambda<GetDigitsDelegate>(digits, value);
        }

        private Expression<GetDigitsDelegate> CreateGetDigitsMonoMethod()
        {
            var value = Expression.Parameter(typeof(Decimal).MakeByRefType(), "value");

            var digits = Expression.RightShift(
                Expression.And(Expression.Field(value, "flags"), Expression.Constant(UInt32.MaxValue >> 1, typeof(uint))),
                Expression.Constant(16, typeof(int)));

            //return (value.flags & (UInt32.MaxValue >> 1)) >> 16

            return Expression.Lambda<GetDigitsDelegate>(Expression.Convert(digits, typeof(int)), value);
        }
    }

    public static class DecimalExtensions
    {
        public static int GetDigits(this Decimal value)
        {
            return DecimalHelper.Instance.GetDigits(ref value);
        }
    }
}

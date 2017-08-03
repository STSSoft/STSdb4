using System;
using System.Linq.Expressions;
using System.Reflection;

namespace STSdb4.General.Extensions
{
    public delegate int DecimalGetDigitsDelegate(ref Decimal value);
    public delegate void DecimalWriteDelegate(ref Decimal value, int[] buffer, int index);

    public class DecimalHelper
    {
        public static readonly DecimalHelper Instance = new DecimalHelper();

        public readonly DecimalGetDigitsDelegate GetDigits;

        /// <summary>
        /// Writes a decimal value into an int[] array in the same order as a BinaryWriter - d.lo, d.mid, d.hi, d.flags.
        /// </summary>
        public readonly DecimalWriteDelegate Write;

        /// <summary>
        /// Create decimal from lo,mid,hi,flags
        /// </summary>
        public readonly Func<int, int, int, int, decimal> Constructor;

        public DecimalHelper()
        {
            var lambda = CreateGetDigitsMethod();
            GetDigits = lambda.Compile();

            Write = CreateWriteMethod().Compile();
            Constructor = CreateConstructorMethod().Compile();
        }

        public Expression<DecimalWriteDelegate> CreateWriteMethod()
        {
            var value = Expression.Parameter(typeof(Decimal).MakeByRefType(), "value");
            var buffer = Expression.Parameter(typeof(int[]), "buffer");
            var index = Expression.Parameter(typeof(int), "index");

            var lo = Expression.Field(value, "lo");
            var mid = Expression.Field(value, "mid");
            var hi = Expression.Field(value, "hi");
            var flags = Expression.Field(value, "flags");

            Expression block = Expression.Block(
                Expression.Assign(Expression.ArrayAccess(buffer, Expression.PostIncrementAssign(index)), lo),
                Expression.Assign(Expression.ArrayAccess(buffer, Expression.PostIncrementAssign(index)), mid),
                Expression.Assign(Expression.ArrayAccess(buffer, Expression.PostIncrementAssign(index)), hi),
                Expression.Assign(Expression.ArrayAccess(buffer, Expression.PostIncrementAssign(index)), flags)
            );

            var lambda = Expression.Lambda<DecimalWriteDelegate>(block, value, buffer, index);

            //private void Write(ref Decimal value, int[] buffer, int index)
            //{
            //    buffer[index++] = value.lo;
            //    buffer[index++] = value.mid;
            //    buffer[index++] = value.hi;
            //    buffer[index++] = value.flags;
            //}

            return lambda;
        }

        private Expression<Func<int, int, int, int, decimal>> CreateConstructorMethod()
        {
            var lo = Expression.Parameter(typeof(int), "lo");
            var mid = Expression.Parameter(typeof(int), "mid");
            var hi = Expression.Parameter(typeof(int), "hi");
            var flags = Expression.Parameter(typeof(int), "flags");

            ConstructorInfo decimalConstructor;

#if NETFX_CORE
            decimalConstructor = typeof(decimal).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
#else
            decimalConstructor = typeof(decimal).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) }, null);
#endif

            var constructor = Expression.New(decimalConstructor, lo, mid, hi, flags);

            //return new Decimal(lo, mid, hi, flags);

            return Expression.Lambda<Func<int, int, int, int, decimal>>(Expression.Label(Expression.Label(typeof(decimal)), constructor), lo, mid, hi, flags);
        }

        private Expression<DecimalGetDigitsDelegate> CreateGetDigitsMethod()
        {
            var value = Expression.Parameter(typeof(Decimal).MakeByRefType(), "value");

            var digits = Expression.RightShift(
                Expression.And(Expression.Field(value, "flags"), Expression.Constant(~Int32.MinValue, typeof(int))),
                Expression.Constant(16, typeof(int)));

            //return (value.flags & ~Int32.MinValue) >> 16

            return Expression.Lambda<DecimalGetDigitsDelegate>(digits, value);
        }
    }

    public static class DecimalExtensions
    {
        public static int GetDigits(this Decimal value)
        {
            return DecimalHelper.Instance.GetDigits(ref value);
        }

        public static void Write(this Decimal value, int[] buffer, int index)
        {
            DecimalHelper.Instance.Write(ref value, buffer, index);
        }
    }
}

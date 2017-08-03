using System.Linq.Expressions;

namespace STSdb4.Data
{
    public static class DataExtensions
    {
        public static Expression Value(this Expression data)
        {
            return Expression.Field(data, "Value");
        }
    }
}

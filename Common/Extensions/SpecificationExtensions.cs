using Common.Types;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Extensions
{
    public static class SpecificationExtensions
    {
        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var invokedExpr = Expression.Invoke(right, left.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, invokedExpr), left.Parameters);
        }

        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            var expression = left.CriteriaExpression.And(right.CriteriaExpression);
            return new PredicateSpecification<T> { PredicateExpression = expression };
        }
    }
}

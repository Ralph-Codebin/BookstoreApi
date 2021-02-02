using System;
using System.Linq.Expressions;

namespace Common.Types
{
    public class PredicateSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> PredicateExpression { get; set; }
        public Expression<Func<T, bool>> CriteriaExpression => PredicateExpression;
    }
}

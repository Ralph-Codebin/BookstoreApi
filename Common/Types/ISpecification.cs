using System;
using System.Linq.Expressions;

namespace Common.Types
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> CriteriaExpression { get; }
    }
}

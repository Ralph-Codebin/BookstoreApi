using Domain.Model.ValueObjects.Abstractions;
using System.Collections.Generic;

namespace Application.Repositories
{
    public interface IValueObjectRepository
    {
        IList<T> List<T>() where T : class, IValueObject;
        bool IsRepositoryFor<T>() where T : IValueObject;
        int Priority { get; }
    }
}

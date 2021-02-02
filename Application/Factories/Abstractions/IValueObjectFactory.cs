using Domain.Model.ValueObjects.Abstractions;
using System.Collections.Generic;

namespace Application.Factories.Abstractions
{
    public interface IValueObjectFactory
    {
        T Get<T>(string code) where T : class, IValueObject;
        IList<T> List<T>() where T : class, IValueObject;
        IList<IValueObject> List(string typeName);
        IEnumerable<string> ListTypes();
    }
}

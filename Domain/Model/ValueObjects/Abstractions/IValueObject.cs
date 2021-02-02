using Common.Types;
using System.Collections.Generic;

namespace Domain.Model.ValueObjects.Abstractions
{
    public interface IValueObject
    {
        LowercaseString Code { get; }
        IEnumerable<LanguageResource<string>> Description { get; }
        IEnumerable<LanguageResource<int>> Sequence { get; }
    }
}

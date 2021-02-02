using Common.Types;
using Domain.Model.ValueObjects.Abstractions;
using System.Collections.Generic;

namespace Domain.Model.ValueObjects
{
    public class Language : ValueObject
    {
        public const string Afrikaans = "Af";
        public const string English = "En";

        public override LowercaseString Code { get; internal set; }
        public override IEnumerable<LanguageResource<string>> Description { get; internal set; }
        public override IEnumerable<LanguageResource<int>> Sequence { get; internal set; }
    }
}

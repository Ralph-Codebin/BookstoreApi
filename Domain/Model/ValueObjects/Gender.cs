using Common.Types;
using Domain.Model.ValueObjects.Abstractions;
using System.Collections.Generic;

namespace Domain.Model.ValueObjects
{
    public class Gender : ValueObject
    {
        public const string Female = "Female";
        public const string Male = "Male";
        public const string NA = "NA";

        public override LowercaseString Code { get; internal set; }
        public override IEnumerable<LanguageResource<string>> Description { get; internal set; }
        public override IEnumerable<LanguageResource<int>> Sequence { get; internal set; }
        public bool IsMale { get; internal set; }
        public bool IsFemale { get; internal set; }
    }
}

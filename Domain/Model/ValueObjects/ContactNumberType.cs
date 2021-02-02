using Common.Types;
using Domain.Model.ValueObjects.Abstractions;
using System.Collections.Generic;

namespace Domain.Model.ValueObjects
{
    public class ContactNumberType : ValueObject
    {
        public const string HomePhone = "HomePhone";
        public const string WorkPhone = "WorkPhone";
        public const string CellPhone = "CellPhone";

        public override LowercaseString Code { get; internal set; }
        public override IEnumerable<LanguageResource<string>> Description { get; internal set; }
        public override IEnumerable<LanguageResource<int>> Sequence { get; internal set; }
    }
}

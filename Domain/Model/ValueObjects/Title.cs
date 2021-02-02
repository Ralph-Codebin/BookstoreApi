using Common.Types;
using Domain.Model.ValueObjects.Abstractions;
using System.Collections.Generic;

namespace Domain.Model.ValueObjects
{
    public class Title : ValueObject
    {
        public const string Professor = "Prof";
        public const string Miss = "Ms";
        public const string Mister = "Mr";
        public const string Doctor = "Dr";
        public const string Mrs = "Mrs";
        public const string Advocate = "Adv";
        public const string Reverend = "Rev";
        public const string Judge = "Judge";

        public override LowercaseString Code { get; internal set; }
        public override IEnumerable<LanguageResource<string>> Description { get; internal set; }
        public override IEnumerable<LanguageResource<int>> Sequence { get; internal set; }
        public bool IsMale { get; internal set; }
        public bool IsFemale { get; internal set; }
    }
}

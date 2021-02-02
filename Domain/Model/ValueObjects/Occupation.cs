using Common.Types;
using Domain.Model.ValueObjects.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model.ValueObjects
{
    public class Occupation : ValueObject
    {
        public Occupation(
            string code,
            List<Tuple<string, string>> descriptions)
        {
            Code = code;
            Description = descriptions.Select(s => new LanguageResource<string> { LanguageCode = s.Item1, Value = s.Item2 });
            Sequence = new List<LanguageResource<int>>();
        }
        public override LowercaseString Code { get; internal set; }
        public override IEnumerable<LanguageResource<string>> Description { get; internal set; }
        public override IEnumerable<LanguageResource<int>> Sequence { get; internal set; }

        public void SetSequence(string languageCode, int sequence)
        {
            var existingSequence = Sequence.ToList();
            var existingLanguage = existingSequence.SingleOrDefault(s => s.LanguageCode == languageCode);
            if(existingLanguage != null)
            {
                existingLanguage.Value = sequence;
            }
            else
            {
                existingSequence.Add(new LanguageResource<int> { LanguageCode = languageCode, Value = sequence });
            }
            Sequence = existingSequence;
        }
    }
}

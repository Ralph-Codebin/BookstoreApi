using Common.Types;
using System.Collections.Generic;

namespace Domain.Model.ValueObjects.Abstractions
{
    public abstract class ValueObject : IValueObject
    {
        public abstract LowercaseString Code { get; internal set; }
        public abstract IEnumerable<LanguageResource<string>> Description { get; internal set; }
        public abstract IEnumerable<LanguageResource<int>> Sequence { get; internal set; }

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (ValueObject)obj;
            if (Code is null ^ other.Code is null)
            {
                return false;
            }
            if (!(Code is null) && !Code.Equals(other.Code))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}

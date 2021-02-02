using Common.Guards;

namespace Common.Types
{
    public class NotEmptyString
    {
        private string _value;

        public NotEmptyString(string value)
        {
            Ensure.ArgumentNotNullOrEmpty(value, nameof(value));
            _value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                Ensure.ArgumentNotNullOrEmpty(value, nameof(value));
                _value = value;
            }
        }

        public static implicit operator string(NotEmptyString nns)
        {
            return nns._value;
        }

        public static implicit operator NotEmptyString(string val)
        {
            return new NotEmptyString(val);
        }

        public override string ToString()
        {
            return _value;
        }

        protected static bool EqualOperator(NotEmptyString left, NotEmptyString right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }

        protected static bool NotEqualOperator(NotEmptyString left, NotEmptyString right)
        {
            return !EqualOperator(left, right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (NotEmptyString)obj;
            if (_value is null ^ other._value is null)
            {
                return false;
            }
            if (!(_value is null) && !_value.Equals(other._value))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return _value?.GetHashCode() ?? 0;
        }
    }
}

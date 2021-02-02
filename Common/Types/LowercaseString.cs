namespace Common.Types
{
    public class LowercaseString
    {
        private NotEmptyString _value;

        public LowercaseString(string value)
        {
            _value = value?.ToLower();
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value?.ToLower();
            }
        }

        public static implicit operator string(LowercaseString lcs)
        {
            return lcs._value;
        }

        public static implicit operator LowercaseString(string val)
        {
            return new LowercaseString(val);
        }

        public override string ToString()
        {
            return _value;
        }

        protected static bool EqualOperator(LowercaseString left, LowercaseString right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }

        protected static bool NotEqualOperator(LowercaseString left, LowercaseString right)
        {
            return !EqualOperator(left, right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (LowercaseString)obj;
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
            return _value.GetHashCode();
        }
    }
}

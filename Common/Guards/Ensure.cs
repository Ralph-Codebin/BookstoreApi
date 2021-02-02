using System;
using System.Collections;
using System.Diagnostics;

namespace Common.Guards
{
    [DebuggerStepThrough]
    public static class Ensure
    {
        public static void ArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(IList argument, string argumentName)
        {
            ArgumentNotNull(argument, argumentName);
            if (argument.Count < 1)
            {
                throw new ArgumentException("Collection cannot be empty", argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void GreaterThanZero(int argument, string argumentName)
        {
            if (argument <= 0)
            {
                throw new ArgumentException("Value must be greater than 0", argumentName);
            }
        }

        public static void True(Func<bool> predicateFunction, string errorWhenFalse)
        {
            if (predicateFunction == null || !predicateFunction())
            {
                throw new Exception(errorWhenFalse);
            }
        }

        public static void Is<T>(object argument, string argumentName)
        {
            if(argument == null || !(argument is T))
            {
                throw new ArgumentException($"{argumentName} must be of type {typeof(T)}");
            }
        }

        public static Because Because(string conditionDescription)
        {
            return new Because(conditionDescription);
        }
    }
}

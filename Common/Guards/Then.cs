using System;

namespace Common.Guards
{
    public class Then
    {
        public void Evaluate(Func<bool> conditionFunction, Func<bool> predicateFunction, string errorMessage)
        {
            if (conditionFunction())
            {
                Ensure.True(predicateFunction, errorMessage);
            }
        }
    }
}

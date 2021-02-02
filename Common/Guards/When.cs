using System;

namespace Common.Guards
{
    public class When
    {
        private readonly string _conditionDescription;
        private readonly Func<bool> _conditionFunction;

        public When(string conditionDescription, Func<bool> conditionFunction)
        {
            _conditionDescription = conditionDescription;
            _conditionFunction = conditionFunction;
        }

        public void Then(Func<bool> predicateFunction)
        {
            new Then().Evaluate(_conditionFunction, predicateFunction, _conditionDescription);
        }
    }
}

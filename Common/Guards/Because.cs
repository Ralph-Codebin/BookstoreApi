using System;

namespace Common.Guards
{
    public class Because
    {
        private readonly string _conditionDescription;

        public Because(string conditionDescription)
        {
            _conditionDescription = conditionDescription;
        }

        public When When(Func<bool> conditionFunction)
        {
            return new When(_conditionDescription, conditionFunction);
        }

        public void Then(Func<bool> predicateFunction)
        {
            new Then().Evaluate(() => true, predicateFunction, _conditionDescription);
        }
    }
}

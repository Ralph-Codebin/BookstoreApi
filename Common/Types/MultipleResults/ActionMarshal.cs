using System;
using System.Diagnostics;

namespace SanQuoteWeb.Common.Types.MultipleResults
{
    [DebuggerStepThrough]
    public class ActionMarshal<TResult1>
    {
        private readonly TResult1 _result1;
        internal ActionMarshal(TResult1 result)
        {
            _result1 = result;
        }

        /// <summary>
        /// Maps an action to a return type
        /// </summary>
        /// <param name="action">The action to perform</param>
        public void Map(Action<TResult1> action)
        {
            if (_result1 != null)
            {
                action?.Invoke(_result1);
            }
        }
    }

    [DebuggerStepThrough]
    public class ActionMarshal<TResult1, TResult2>
    {
        private readonly TResult1 _result1;
        private readonly TResult2 _result2;

        #region Constructors

        internal ActionMarshal(TResult1 result1, TResult2 result2)
        {
            _result1 = result1;
            _result2 = result2;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps an action to a return type
        /// </summary>
        /// <param name="action">The action to perform if the result is of the first return type</param>
        /// <returns>Another ActionMarshal that exposes the other return type</returns>
        public ActionMarshal<TResult2> Map(Action<TResult1> action)
        {
            return Map(_result1, action, _result2);
        }

        /// <summary>
        /// Maps an action to a return type
        /// </summary>
        /// <param name="action">The action to perform if the result is of the second return type</param>
        /// <returns>Another ActionMarshal that exposes the other return type</returns>
        public ActionMarshal<TResult1> Map(Action<TResult2> action)
        {
            return Map(_result2, action, _result1);
        }

        #endregion

        #region Private Methods

        private static ActionMarshal<TOther1> Map<TThis, TOther1>(
            TThis thisResult,
            Action<TThis> thisAction,
            TOther1 otherResult1)
        {
            new ActionMarshal<TThis>(thisResult).Map(thisAction);
            return new ActionMarshal<TOther1>(otherResult1);
        }

        #endregion
    }
}

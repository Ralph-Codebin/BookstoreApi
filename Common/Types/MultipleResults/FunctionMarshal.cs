using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SanQuoteWeb.Common.Types.MultipleResults
{
    [DebuggerStepThrough]
    public class FunctionMarshal<TResult1, TInvokerResult>
    {
        private readonly TResult1 _result1;
        private readonly TInvokerResult _invokerResult;
        internal FunctionMarshal(TResult1 result, TInvokerResult invokerResult)
        {
            _result1 = result;
            _invokerResult = invokerResult;
        }

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute</param>
        public TInvokerResult Map(Func<TResult1, TInvokerResult> function)
        {
            if (_invokerResult != null)
            {
                return _invokerResult;
            }
            if (_result1 != null && function != null)
            {
                return function(_result1);
            }
            return default(TInvokerResult);
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="asyncFunction">The async function to execute</param>
        public async Task<TInvokerResult> MapAsync(Func<TResult1, Task<TInvokerResult>> asyncFunction)
        {
            if (_invokerResult != null)
            {
                return _invokerResult;
            }
            if (_result1 != null && asyncFunction != null)
            {
                return await asyncFunction(_result1);
            }
            return default(TInvokerResult);
        }

    }

    [DebuggerStepThrough]
    public class FunctionMarshal<TResult1, TResult2, TInvokerResult> where TInvokerResult : class
    {
        private readonly TResult1 _result1;
        private readonly TResult2 _result2;
        private readonly TInvokerResult _invokerResult;

        #region Constructors

        internal FunctionMarshal(TResult1 result1, TResult2 result2, TInvokerResult invokerResult)
        {
            _result1 = result1;
            _result2 = result2;
            _invokerResult = invokerResult;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the first return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public FunctionMarshal<TResult2, TInvokerResult> Map(Func<TResult1, TInvokerResult> function)
        {
            return Map(_result1, function, _result2);
        }

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the second return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public FunctionMarshal<TResult1, TInvokerResult> Map(Func<TResult2, TInvokerResult> function)
        {
            return Map(_result2, function, _result1);
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the first return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public async Task<FunctionMarshal<TResult2, TInvokerResult>> MapAsync(Func<TResult1, Task<TInvokerResult>> function)
        {
            return await MapAsync(_result1, function, _result2);
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the second return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public async Task<FunctionMarshal<TResult1, TInvokerResult>> MapAsync(Func<TResult2, Task<TInvokerResult>> function)
        {
            return await MapAsync(_result2, function, _result1);
        }

        #endregion

        #region Private Methods

        private FunctionMarshal<TOther1, TInvokerResult> Map<TThis, TOther1>(
            TThis thisResult,
            Func<TThis, TInvokerResult> thisFunction,
            TOther1 otherResult1)
        {
            var invokerResult = new FunctionMarshal<TThis, TInvokerResult>(thisResult, _invokerResult).Map(thisFunction);
            return new FunctionMarshal<TOther1, TInvokerResult>(otherResult1, _invokerResult ?? invokerResult);
        }

        private async Task<FunctionMarshal<TOther1, TInvokerResult>> MapAsync<TThis, TOther1>(
           TThis thisResult,
           Func<TThis, Task<TInvokerResult>> thisAsyncFunction,
           TOther1 otherResult1)
        {
            var invokerResult = await new FunctionMarshal<TThis, TInvokerResult>(thisResult, _invokerResult).MapAsync(thisAsyncFunction);
            return new FunctionMarshal<TOther1, TInvokerResult>(otherResult1, _invokerResult ?? invokerResult);
        }

        #endregion

    }
}

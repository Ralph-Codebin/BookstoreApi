using Common.Guards;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SanQuoteWeb.Common.Types.MultipleResults
{
    /// <summary>
    /// Allows the return of multiple result types
    /// </summary>
    /// <remarks>
    /// Result types must not inherit from one another
    /// </remarks>
    /// <typeparam name="TResult1">The first result type</typeparam>
    /// <typeparam name="TResult2">The second result type</typeparam>
    [DebuggerStepThrough]
    public class ResultMarshal<TResult1, TResult2>
    {
        private readonly TResult1 _result1;
        private readonly TResult2 _result2;

        #region Constructors

        public ResultMarshal(TResult1 result)
        {
            GuardAgainstIncorrectGenericConstructorResolution();
            Ensure.ArgumentNotNull(result, nameof(result));
            _result1 = result;
        }

        public ResultMarshal(TResult2 result)
        {
            GuardAgainstIncorrectGenericConstructorResolution();
            Ensure.ArgumentNotNull(result, nameof(result));
            _result2 = result;
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

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the first return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public FunctionMarshal<TResult2, T> Map<T>(Func<TResult1, T> function) where T : class
        {
            return Map(_result1, function, _result2);
        }

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the second return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public FunctionMarshal<TResult1, T> Map<T>(Func<TResult2, T> function) where T : class
        {
            return Map(_result2, function, _result1);
        }

        public TTo MapAll<TFrom, TTo>(Func<TFrom, TTo> function)
           where TFrom : class where TTo : class
        {
            var result1 = new FunctionMarshal<TFrom, TTo>(_result1 as TFrom, null).Map(function);
            var result2 = new FunctionMarshal<TFrom, TTo>(_result2 as TFrom, result1).Map(function);
            return result2;
        }

        /// <summary>
        /// Will convert both result types into another type and execute the same function
        /// regardless of which result was returned
        /// </summary>
        /// <typeparam name="T">The type to convert the results into</typeparam>
        /// <param name="function">The function to execute on the new result type</param>
        /// <returns>The functions result as the new result type</returns>
        public T MapAll<T>(Func<T, T> function)
           where T : class
        {
            var result1 = new FunctionMarshal<T, T>(_result1 as T, null).Map(function);
            var result2 = new FunctionMarshal<T, T>(_result2 as T, result1).Map(function);
            return result2;
        }

        /// <summary>
        /// Will convert both result types into another type and execute the same async function
        /// regardless of which result was returned
        /// </summary>
        /// <typeparam name="T">The type to convert the results into</typeparam>
        /// <param name="function">The async function to execute on the new result type</param>
        /// <returns>The functions result as the new result type</returns>
        public async Task<T> MapAllAsync<T>(Func<T, Task<T>> function)
           where T : class
        {
            var result1 = await new FunctionMarshal<T, T>(_result1 as T, null).MapAsync(function);
            var result2 = await new FunctionMarshal<T, T>(_result2 as T, result1).MapAsync(function);
            return result2;
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the first return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public async Task<FunctionMarshal<TResult2, T>> MapAsync<T>(Func<TResult1, Task<T>> function) where T : class
        {
            return await MapAsync(_result1, function, _result2);
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the second return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return type</returns>
        public async Task<FunctionMarshal<TResult1, T>> MapAsync<T>(Func<TResult2, Task<T>> function) where T : class
        {
            return await MapAsync(_result2, function, _result1);
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

        private static FunctionMarshal<TOther1, TInvokerReturn> Map<TThis, TOther1, TInvokerReturn>(
            TThis thisResult,
            Func<TThis, TInvokerReturn> thisFunction,
            TOther1 otherResult1) where TInvokerReturn : class
        {
            var invokerResult = new FunctionMarshal<TThis, TInvokerReturn>(thisResult, null).Map(thisFunction);
            return new FunctionMarshal<TOther1, TInvokerReturn>(otherResult1, invokerResult);
        }

        private static async Task<FunctionMarshal<TOther1, TInvokerResult>> MapAsync<TThis, TOther1, TInvokerResult>(
           TThis thisResult,
           Func<TThis, Task<TInvokerResult>> thisAsyncFunction,
           TOther1 otherResult1) where TInvokerResult : class
        {
            var invokerResult = await new FunctionMarshal<TThis, TInvokerResult>(thisResult, null).MapAsync(thisAsyncFunction);
            return new FunctionMarshal<TOther1, TInvokerResult>(otherResult1, invokerResult);
        }

        private static void GuardAgainstIncorrectGenericConstructorResolution()
        {
            ResultMarshalGuard.GuardAgainstRelationship<TResult1, TResult2>();
        }

        #endregion
    }


    /// <summary>
    /// Allows the return of multiple result types
    /// <remarks>
    /// Result types must not inherit from one another
    /// </remarks>
    /// </summary>
    /// <typeparam name="TResult1">The first result type</typeparam>
    /// <typeparam name="TResult2">The second result type</typeparam>
    /// <typeparam name="TResult3">The third result type</typeparam>
    [DebuggerStepThrough]
    public class ResultMarshal<TResult1, TResult2, TResult3>
    {
        private readonly TResult1 _result1;
        private readonly TResult2 _result2;
        private readonly TResult3 _result3;

        #region Constructors

        public ResultMarshal(TResult1 result)
        {
            GuardAgainstIncorrectGenericConstructorResolution();
            Ensure.ArgumentNotNull(result, nameof(result));
            _result1 = result;
        }
        public ResultMarshal(TResult2 result)
        {
            GuardAgainstIncorrectGenericConstructorResolution();
            Ensure.ArgumentNotNull(result, nameof(result));
            _result2 = result;
        }
        public ResultMarshal(TResult3 result)
        {
            GuardAgainstIncorrectGenericConstructorResolution();
            Ensure.ArgumentNotNull(result, nameof(result));
            _result3 = result;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps an action to a return type
        /// </summary>
        /// <param name="action">The action to perform if the result is of the first return type</param>
        /// <returns>Another ActionMarshal that exposes the other return types</returns>
        public ActionMarshal<TResult2, TResult3> Map(Action<TResult1> action)
        {
            return Map(_result1, action, _result2, _result3);
        }

        /// <summary>
        /// Maps an action to a return type
        /// </summary>
        /// <param name="action">The action to perform if the result is of the second return type</param>
        /// <returns>Another ActionMarshal that exposes the other return types</returns>
        public ActionMarshal<TResult1, TResult3> Map(Action<TResult2> action)
        {
            return Map(_result2, action, _result1, _result3);
        }

        /// <summary>
        /// Maps an action to a return type
        /// </summary>
        /// <param name="action">The action to perform if the result is of the third return type</param>
        /// <returns>Another ActionMarshal that exposes the other return types</returns>
        public ActionMarshal<TResult1, TResult2> Map(Action<TResult3> action)
        {
            return Map(_result3, action, _result1, _result2);
        }

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the first return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return types</returns>
        public FunctionMarshal<TResult2, TResult3, T> Map<T>(Func<TResult1, T> function) where T : class
        {
            return Map(_result1, function, _result2, _result3);
        }

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the second return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return types</returns>
        public FunctionMarshal<TResult1, TResult3, T> Map<T>(Func<TResult2, T> function) where T : class
        {
            return Map(_result2, function, _result1, _result3);
        }

        /// <summary>
        /// Maps a function to a return type
        /// </summary>
        /// <param name="function">The function to execute if the result is of the third return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return types</returns>
        public FunctionMarshal<TResult1, TResult2, T> Map<T>(Func<TResult3, T> function) where T : class
        {
            return Map(_result3, function, _result1, _result2);
        }

        public TTo MapAll<TFrom, TTo>(Func<TFrom, TTo> function)
           where TFrom : class where TTo : class
        {
            var result1 = new FunctionMarshal<TFrom, TTo>(_result1 as TFrom, null).Map(function);
            var result2 = new FunctionMarshal<TFrom, TTo>(_result2 as TFrom, result1).Map(function);
            var result3 = new FunctionMarshal<TFrom, TTo>(_result3 as TFrom, result2).Map(function);
            return result3;
        }

        /// <summary>
        /// Will convert both result types into another type and execute the same function
        /// regardless of which result was returned
        /// </summary>
        /// <typeparam name="T">The type to convert the results into</typeparam>
        /// <param name="function">The function to execute on the new result type</param>
        /// <returns>The functions result as the new result type</returns>
        public T MapAll<T>(Func<T, T> function)
           where T : class
        {
            var result1 = new FunctionMarshal<T, T>(_result1 as T, null).Map(function);
            var result2 = new FunctionMarshal<T, T>(_result2 as T, result1).Map(function);
            var result3 = new FunctionMarshal<T, T>(_result3 as T, result2).Map(function);
            return result3;
        }

        /// <summary>
        /// Will convert both result types into another type and execute the same function
        /// regardless of which result was returned
        /// </summary>
        /// <typeparam name="T">The type to convert the results into</typeparam>
        /// <param name="function">The function to execute on the new result type</param>
        /// <returns>The functions result as the new result type</returns>
        public async Task<T> MapAllAsync<T>(Func<T, Task<T>> function)
           where T : class
        {
            var result1 = await new FunctionMarshal<T, T>(_result1 as T, null).MapAsync(function);
            var result2 = await new FunctionMarshal<T, T>(_result2 as T, result1).MapAsync(function);
            var result3 = await new FunctionMarshal<T, T>(_result3 as T, result2).MapAsync(function);
            return result3;
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the first return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return types</returns>
        public async Task<FunctionMarshal<TResult2, TResult3, T>> MapAsync<T>(Func<TResult1, Task<T>> function) where T : class
        {
            return await MapAsync(_result1, function, _result2, _result3);
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the second return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return types</returns>
        public async Task<FunctionMarshal<TResult1, TResult3, T>> MapAsync<T>(Func<TResult2, Task<T>> function) where T : class
        {
            return await MapAsync(_result2, function, _result1, _result3);
        }

        /// <summary>
        /// Maps an async function to a return type
        /// </summary>
        /// <param name="function">The async function to execute if the result is of the third return type</param>
        /// <returns>Another FunctionMarshal that exposes the other return types</returns>
        public async Task<FunctionMarshal<TResult1, TResult2, T>> MapAsync<T>(Func<TResult3, Task<T>> function) where T : class
        {
            return await MapAsync(_result3, function, _result1, _result2);
        }

        #endregion

        #region Private Methods

        private static ActionMarshal<TOther1, TOther2> Map<TThis, TOther1, TOther2>(
            TThis thisResult,
            Action<TThis> thisAction,
            TOther1 otherResult1,
            TOther2 otherResult2)
        {
            new ActionMarshal<TThis>(thisResult).Map(thisAction);
            return new ActionMarshal<TOther1, TOther2>(otherResult1, otherResult2);
        }

        private static FunctionMarshal<TOther1, TOther2, TInvokerReturn> Map<TThis, TOther1, TOther2, TInvokerReturn>(
            TThis thisResult,
            Func<TThis, TInvokerReturn> thisFunction,
            TOther1 otherResult1,
            TOther2 otherResult2) where TInvokerReturn : class
        {
            var invokerResult = new FunctionMarshal<TThis, TInvokerReturn>(thisResult, null).Map(thisFunction);
            return new FunctionMarshal<TOther1, TOther2, TInvokerReturn>(otherResult1, otherResult2, invokerResult);
        }

        private static async Task<FunctionMarshal<TOther1, TOther2, TInvokerResult>> MapAsync<TThis, TOther1, TOther2, TInvokerResult>(
           TThis thisResult,
           Func<TThis, Task<TInvokerResult>> thisAsyncFunction,
           TOther1 otherResult1,
           TOther2 otherResult2) where TInvokerResult : class
        {
            var invokerResult = await new FunctionMarshal<TThis, TInvokerResult>(thisResult, null).MapAsync(thisAsyncFunction);
            return new FunctionMarshal<TOther1, TOther2, TInvokerResult>(otherResult1, otherResult2, invokerResult);
        }

        private static void GuardAgainstIncorrectGenericConstructorResolution()
        {
            ResultMarshalGuard.GuardAgainstRelationship<TResult1, TResult2>();
            ResultMarshalGuard.GuardAgainstRelationship<TResult2, TResult3>();
            ResultMarshalGuard.GuardAgainstRelationship<TResult3, TResult1>();
        }

        #endregion
    }
}

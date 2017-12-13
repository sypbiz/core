using System;
using System.Diagnostics;

namespace syp.biz.Core
{
    /// <summary>
    /// Wrapper to perform one-liner exception handling.
    /// </summary>
    public static class Try
    {
        /// <summary>
        /// Executes <paramref name="action"/> and ignores any exception.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        [DebuggerStepThrough]
        public static void Ignore(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                // do nothing
            }
        }

        /// <summary>
        /// Executes <paramref name="func"/>, returning the <typeparamref name="T"/> result, ignoring any exceptions.
        /// </summary>
        /// <typeparam name="T">The result type to return.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <returns>The result of the execution of <paramref name="func"/> or <c>default(T)</c> in case of an exception.</returns>
        [DebuggerStepThrough]
        public static T Ignore<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Executes <paramref name="action"/>, while catching exceptions of <typeparamref name="TEx"/>.<br/>
        /// Any other exceptions will be thrown.
        /// </summary>
        /// <typeparam name="TEx">The exception type to catch.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <param name="exceptionHandler">Optional. A handler for the exception. Default is <c>null</c>, which will ignore the exception.</param>
        [DebuggerStepThrough]
        public static void Catch<TEx>(Action action, Action<TEx> exceptionHandler = null) where TEx : Exception
        {
            try
            {
                action();
            }
            catch (TEx ex)
            {
                exceptionHandler?.Invoke(ex);
            }
        }

        /// <summary>
        /// Executes <paramref name="func"/>, returning the <typeparamref name="T"/> result, while catching exceptions of <typeparamref name="TEx"/>.<br/>
        /// Any other exceptions will be thrown.
        /// </summary>
        /// <typeparam name="TEx">The exception type to catch.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="exceptionHandler">Optional. A handler for the exception. Default is <c>null</c>, which will ignore the exception.</param>
        /// <returns>The result of the execution of <paramref name="func"/> or <c>default(T)</c> in case of an exception of type <typeparamref name="TEx"/>.</returns>
        [DebuggerStepThrough]
        public static T Catch<TEx, T>(Func<T> func, Func<TEx, T> exceptionHandler = null) where TEx : Exception
        {
            try
            {
                return func();
            }
            catch (TEx ex)
            {
                return exceptionHandler == null ? default(T) : exceptionHandler(ex);
            }
        }
    }
}

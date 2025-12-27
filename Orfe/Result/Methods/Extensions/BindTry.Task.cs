using System.Threading.Tasks;
using System;

namespace Orfe;

public static partial class ResultExtensions
{
    /// <param name="resultTask">Extended result</param>
    /// <typeparam name="T">Result Type parameter</typeparam>
    /// <typeparam name="TK"><paramref /> Result Type parameter</typeparam>
    /// <typeparam name="TE">Error Type parameter</typeparam>
    extension<T, TK, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        ///     If a given function throws an exception, an error is returned from the given error handler
        /// </summary>
        /// <param name="func">Function returning result to bind</param>
        /// <param name="errorHandler">Error handling function</param>
        /// <returns>Binding result</returns>
        public async Task<Result<TK, TE>> BindTry(Func<T, Result<TK, TE>> func,
            Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindTry(func, errorHandler);
        }

        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        ///     If a given function throws an exception, an error is returned from the given error handler
        /// </summary>
        /// <param name="func">Function returning result to bind</param>
        /// <param name="errorHandler">Error handling function</param>
        /// <returns>Binding result</returns>
        public async Task<Result<TK, TE>> BindTry(Func<T, Task<Result<TK, TE>>> func,
            Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindTry(func, errorHandler).ConfigureAwait(DefaultConfigureAwait);
        }
    }

    /// <summary>
    ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    ///     If a given function throws an exception, an error is returned from the given error handler
    /// </summary>
    /// <typeparam name="T">Result Type parameter</typeparam>
    /// <typeparam name="TK"><paramref name="func" /> Result Type parameter</typeparam>
    /// <typeparam name="TE">Error Type parameter</typeparam>
    /// <param name="result">Extended result</param>
    /// <param name="func">Function returning result to bind</param>
    /// <param name="errorHandler">Error handling function</param>
    /// <returns>Binding result</returns>
    public static async Task<Result<TK, TE>> BindTry<T, TK, TE>(this Result<T, TE> result, Func<T, Task<Result<TK, TE>>> func,
        Func<Exception, TE> errorHandler)
    {
        return result.IsFailure
            ? Result.Failure<TK, TE>(result.Error)
            : await Result.Try(() => func(result.Value),errorHandler).Bind(r => r).ConfigureAwait(DefaultConfigureAwait);
    }
}


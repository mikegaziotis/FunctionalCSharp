using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        ///     If a given function throws an exception, an error is returned from the given error handler
        /// </summary>
        public async ValueTask<Result<TK, TE>> MapTry(Func<T, ValueTask<TK>> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapTry(func, errorHandler).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        ///     If a given function throws an exception, an error is returned from the given error handler
        /// </summary>
        public async ValueTask<Result<TK, TE>> MapTry(Func<T, TK> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.MapTry(func, errorHandler);
        }
    }

    /// <summary>
    ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    ///     If a given function throws an exception, an error is returned from the given error handler
    /// </summary>
    public static async ValueTask<Result<TK, TE>> MapTry<T, TK, TE>(this Result<T, TE> result, Func<T, ValueTask<TK>> func, Func<Exception, TE> errorHandler)
        => result.IsFailure
            ? Result.Failure<TK, TE>(result.Error)
            : await Result.Try(() => func(result.Value), errorHandler).ConfigureAwait(DefaultConfigureAwait);

}

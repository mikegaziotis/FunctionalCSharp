using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Bind(Func<T, Task<Result<TK, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Bind(Func<T, Result<TK, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Bind(func);
        }

        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Bind(Func<Task<Result<TK, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Bind(Func<Result<TK, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Bind(func);
        }
    }

    extension<T, TK, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Bind(Func<T, Task<Result<TK, TE>>> func)
            => result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : await func(result.Value).ConfigureAwait(false);

        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Bind(Func<Task<Result<TK, TE>>> func)
            => result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : await func().ConfigureAwait(DefaultConfigureAwait);
    }
}

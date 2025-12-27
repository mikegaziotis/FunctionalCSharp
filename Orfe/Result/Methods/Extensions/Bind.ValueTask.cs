using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Selects result from the return value of a given valueTask action. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Bind(Func<T, ValueTask<Result<TK, TE>>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Bind(valueTask).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Selects result from the return value of a given valueTask action. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Bind(Func<ValueTask<Result<TK, TE>>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Bind(valueTask).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Selects result from the return value of a given valueTask action. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Bind(Func<T, Result<TK, TE>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Bind(valueTask);
        }

        /// <summary>
        ///     Selects result from the return value of a given valueTask action. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Bind(Func<Result<TK, TE>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Bind(valueTask);
        }
    }

    extension<T, TK, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Bind(Func<T, ValueTask<Result<TK, TE>>> func)
            => result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

        /// <summary>
        ///     Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Bind(Func<ValueTask<Result<TK, TE>>> func)
            => result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : await func().ConfigureAwait(DefaultConfigureAwait);
    }
}


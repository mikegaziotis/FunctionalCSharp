using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     If the calling Result is a success, a new success result is returned. Otherwise, creates a new failure result from the return value of a given valueTask action.
        /// </summary>
        public async ValueTask<Result<T, TE2>> MapError<TE2>(Func<TE, ValueTask<TE2>> errorFactory)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapError(errorFactory).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE2>> MapError<TE2, TContext>(Func<TE, TContext, ValueTask<TE2>> errorFactory, TContext context)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapError(errorFactory, context).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     If the calling Result is a success, a new success result is returned. Otherwise, creates a new failure result from the return value of a given valueTask action.
        /// </summary>
        public async ValueTask<Result<T, TE2>> MapError<TE2>(Func<TE, TE2> errorFactory)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            if (result.IsSuccess)
                return Result.Success<T, TE2>(result.Value);

            var error = errorFactory(result.Error);
            return Result.Failure<T, TE2>(error);
        }

        public async ValueTask<Result<T, TE2>> MapError<TE2, TContext>(Func<TE, TContext, TE2> errorFactory, TContext context)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            if (result.IsSuccess)
                return Result.Success<T, TE2>(result.Value);

            var error = errorFactory(result.Error, context);
            return Result.Failure<T, TE2>(error);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     If the calling Result is a success, a new success result is returned. Otherwise, creates a new failure result from the return value of a given valueTask action.
        /// </summary>
        public async ValueTask<Result<T, TE2>> MapError<TE2>(Func<TE, ValueTask<TE2>> errorFactory)
        {
            if (result.IsSuccess)
                return Result.Success<T, TE2>(result.Value);

            var error = await errorFactory(result.Error).ConfigureAwait(DefaultConfigureAwait);
            return Result.Failure<T, TE2>(error);
        }

        /// <summary>
        ///     If the calling Result is a success, a new success result is returned. Otherwise, creates a new failure result from the return value of a given valueTask action.
        /// </summary>
        public async ValueTask<Result<T, TE2>> MapError<TE2, TContext>(Func<TE, TContext, ValueTask<TE2>> errorFactory, TContext context)
        {
            if (result.IsSuccess)
                return Result.Success<T, TE2>(result.Value);

            var error = await errorFactory(result.Error, context).ConfigureAwait(DefaultConfigureAwait);
            return Result.Failure<T, TE2>(error);
        }
    }
}

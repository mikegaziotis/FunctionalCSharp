using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Map(Func<T, ValueTask<TK>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Map<TContext>(Func<T, TContext, ValueTask<TK>> func, TContext context)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value, context).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Map(Func<T, TK> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Map(func);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Map<TContext>(Func<T, TContext, TK> func, TContext context)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Map(func, context);
        }
    }

    extension<T, TK, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Map(Func<T, ValueTask<TK>> func)
        {
            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async ValueTask<Result<TK, TE>> Map<TContext>(Func<T, TContext, ValueTask<TK>> func, TContext context)
        {
            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value, context).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }
    }
}

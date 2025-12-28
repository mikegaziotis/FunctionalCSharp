using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Map(Func<T, Task<TK>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Map<TContext>(Func<T, TContext, Task<TK>> func, TContext context)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value, context).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Map(Func<T, TK> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Map(func);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Map<TContext>(Func<T, TContext, TK> func, TContext context)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Map(func, context);
        }
    }

    extension<T, TK, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Map(Func<T, Task<TK>> func)
        {
            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public async Task<Result<TK, TE>> Map<TContext>(Func<T, TContext, Task<TK>> func, TContext context)
        {
            if (result.IsFailure)
                return Result.Failure<TK, TE>(result.Error);

            var value = await func(result.Value, context).ConfigureAwait(DefaultConfigureAwait);

            return Result.Success<TK, TE>(value);
        }
    }
}

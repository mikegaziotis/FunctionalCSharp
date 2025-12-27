using System;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public Result<TK, TE> Map(Func<T, TK> func)
            => result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : Result.Success<TK, TE>(func(result.Value));

        /// <summary>
        ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public Result<TK, TE> Map<TContext>(Func<T, TContext, TK> func, TContext context)
            => result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : Result.Success<TK, TE>(func(result.Value, context));
    }
}

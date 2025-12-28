using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     If the calling Result is a success, a new success result is returned. Otherwise, creates a new failure result from the return value of a given function.
        /// </summary>
        public Result<T, TE2> MapError<TE2>(Func<TE, TE2> errorFactory)
            =>  result.IsFailure
                ? Result.Failure<T, TE2>(errorFactory(result.Error))
                : Result.Success<T, TE2>(result.Value);

        public Result<T, TE2> MapError<TE2, TContext>(Func<TE, TContext, TE2> errorFactory, TContext context)
            =>  result.IsFailure
                ? Result.Failure<T, TE2>(errorFactory(result.Error, context))
                : Result.Success<T, TE2>(result.Value);
    }
}

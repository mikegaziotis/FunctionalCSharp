using System;

namespace Orfe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     Creates a new result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    ///     If a given function throws an exception, an error is returned from the given error handler
    /// </summary>
    public static Result<TK, TE> MapTry<T, TK, TE>(this Result<T, TE> result, Func<T, TK> func, Func<Exception, TE> errorHandler)
        => result.IsFailure
            ? Result.Failure<TK, TE>(result.Error)
            : Result.Try(() => func(result.Value), errorHandler);
}

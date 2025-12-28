using System;

namespace Roufe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     If the given result is a success returns a new success result. Otherwise, it returns the result of the given function.
    /// </summary>
    public static Result<T, TE2> Compensate<T, TE, TE2>(this Result<T, TE> result, Func<TE, Result<T, TE2>> func)
        => result.IsSuccess ? Result.Success<T, TE2>(result.Value) : func(result.Error);
}

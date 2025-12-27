using System;

namespace Orfe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     Returns a new failure result if the predicate is true. Otherwise, returns the starting result.
    /// </summary>
    public static Result<T, TE> EnsureNot<T, TE>(this Result<T, TE> result, Func<T, bool> test, TE error) =>
        result.Ensure(v => !test(v), error);
}

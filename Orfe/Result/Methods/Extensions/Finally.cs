using System;

namespace Orfe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     Passes the result to the given function (regardless of success/failure state) to yield a final output value.
    /// </summary>
    /// <summary>
    ///     Passes the result to the given function (regardless of success/failure state) to yield a final output value.
    /// </summary>
    public static TK Finally<T, TK, TE>(this Result<T, TE> result, Func<Result<T, TE>, TK> func)
        => func(result);
}

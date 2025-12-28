using System;

namespace Roufe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     This method should be used in linq queries. We recommend using Bind method.
    /// </summary>
    public static Result<TR, TE> SelectMany<T, TK, TE, TR>(this Result<T, TE> result, Func<T, Result<TK, TE>> func, Func<T, TK, TR> project)
        => result
            .Bind(func)
            .Map(x => project(result.Value, x));
}

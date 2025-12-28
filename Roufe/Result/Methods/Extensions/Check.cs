using System;

namespace Roufe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     If the calling result is a success, the given function is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
    /// </summary>
    public static Result<T, TE> Check<T, TK, TE>(this Result<T, TE> result, Func<T, Result<TK, TE>> func)
        => result.Bind(func).Map(_ => result.Value);

}

using System;

namespace Roufe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     This method should be used in linq queries. We recommend using Map method.
    /// </summary>
    public static Result<TK,TE> Select<T, TK,TE>(in this Result<T,TE> result, Func<T, TK> selector)
        => result.Map(selector);
}

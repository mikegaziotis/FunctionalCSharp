using System;

namespace Roufe;

public static partial class ResultExtensions
{
    /// <summary>
    ///    Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    ///    If a given function throws an exception, an error is returned from the given error handler
    /// </summary>
    /// <typeparam name="T">Result Type parameter</typeparam>
    /// <typeparam name="TK"><paramref name="func" /> Result Type parameter</typeparam>
    /// <typeparam name="TE">Error Type parameter</typeparam>
    /// <param name="result">Extended result</param>
    /// <param name="func">Function returning result to bind</param>
    /// <param name="errorHandler">Error handling function</param>
    /// <returns>Binding result</returns>
    public static Result<TK, TE> BindTry<T, TK, TE>(this Result<T, TE> result, Func<T, Result<TK, TE>> func,
        Func<Exception, TE> errorHandler)
    {
        return result.IsFailure
            ? Result.Failure<TK,TE>(result.Error)
            : Result.Try(() => func(result.Value), errorHandler).Bind(r => r);
    }
}

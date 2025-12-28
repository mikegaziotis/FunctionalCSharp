using System;
using System.Collections.Generic;
using System.Linq;

namespace Roufe;

public static partial class ResultExtensions
{
    public static Result<IEnumerable<T>, TE> Combine<T, TE>(this IEnumerable<Result<T, TE>> results) where TE : ICombine
    {
        results = results.ToList();
        var result = Result.Combine(results);

        return result.IsSuccess
            ? Result.Success<IEnumerable<T>, TE>(results.Select(e => e.Value))
            : Result.Failure<IEnumerable<T>, TE>(result.Error);
    }

    public static Result<TK, TE> Combine<T, TK, TE>(this IEnumerable<Result<T, TE>> results, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        var result = results.Combine();

        return result.IsSuccess
            ? Result.Success<TK, TE>(composer(result.Value))
            : Result.Failure<TK, TE>(result.Error);
    }

    public static Result<IEnumerable<T>, TE> Combine<T, TE>(this IEnumerable<Result<T, TE>> results, Func<IEnumerable<TE>, TE> composerError)
    {
        results = results.ToList();
        var result = Result.Combine(results, composerError);

        return result.IsSuccess
            ? Result.Success<IEnumerable<T>, TE>(results.Select(e => e.Value))
            : Result.Failure<IEnumerable<T>, TE>(result.Error);
    }
    public static Result<TK, TE> Combine<T, TK, TE>(this IEnumerable<Result<T, TE>> results, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        Result<IEnumerable<T>, TE> result = results.Combine(composerError);

        return result.IsSuccess
            ? Result.Success<TK, TE>(composer(result.Value))
            : Result.Failure<TK, TE>(result.Error);
    }


}

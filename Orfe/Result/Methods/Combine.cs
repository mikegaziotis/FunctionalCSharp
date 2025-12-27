using System;
using System.Collections.Generic;
using System.Linq;

namespace Orfe;

public partial struct Result
{
    /// <summary>
    ///     Combines several results (and any errors) into a single result.
    ///     The returned result will be a failure if any of the input <paramref name="results"/> are failures.
    /// </summary>
    /// <param name="results">
    ///     The Results to be combined.</param>
    /// <param name="composerError">
    ///     A function that combines any errors.</param>
    /// <returns>
    ///     A Result that is a success when all the input <paramref name="results"/> are also successes.</returns>
    public static Result<bool,TE> Combine<T,TE>(IEnumerable<Result<T,TE>> results, Func<IEnumerable<TE>, TE> composerError)
    {
        var failedResults = results.Where(x => x.IsFailure).ToList();

        if (failedResults.Count == 0)
            return Success<bool,TE>(true);

        var error = composerError(failedResults.Select(x => x.Error));
        return Failure<bool,TE>(error);
    }

    /// <summary>
    ///     Combines several results (and any errors) into a single result.
    ///     The returned result will be a failure if any of the input <paramref name="results"/> are failures.
    ///     The E error class must implement ICombine to provide an accumulator function for combining any errors.
    ///     NB: The bool value type is arbitrary - the value is not intended to be used.</summary>
    /// <param name="results">
    ///     The Results to be combined.</param>
    /// <returns>
    ///     A Result that is a success when all the input <paramref name="results"/> are also successes.</returns>
    public static Result<bool, TE> Combine<T, TE>(IEnumerable<Result<T, TE>> results)
        where TE : ICombine
        => Combine(results, CombineErrors);

    /// <summary>
    ///     Combines several results (and any errors) into a single result.
    ///     The returned result will be a failure if any of the input <paramref name="results"/> are failures.
    ///     The E error class must implement ICombine to provide an accumulator function for combining any errors.
    ///     NB: The bool value type is arbitrary - the result Value is not intended to be used.</summary>
    /// <param name="results">
    ///     The Results to be combined.</param>
    /// <returns>
    ///     A Result that is a success when all the input <paramref name="results"/> are also successes.</returns>
    public static Result<bool, TE> Combine<T, TE>(params Result<T, TE>[] results)
        where TE : ICombine
        => Combine(results, CombineErrors);


    /// <summary>
    ///     Combines several results (and any errors) into a single result.
    ///     The returned result will be a failure if any of the input <paramref name="results"/> are failures.
    ///     NB: The bool value type is arbitrary - the result Value is not intended to be used.</summary>
    /// <param name="composerError">
    ///     A function that combines any errors.</param>
    /// <param name="results">
    ///     The Results to be combined.</param>
    /// <returns>
    ///     A Result that is a success when all the input <paramref name="results"/> are also successes.</returns>
    public static Result<bool, TE> Combine<T, TE>(Func<IEnumerable<TE>, TE> composerError, params Result<T, TE>[] results)
        => Combine(results, composerError);

    private static TE CombineErrors<TE>(IEnumerable<TE> errors)
        where TE : ICombine
        => errors.Aggregate((x, y) => (TE)x.Combine(y));
}

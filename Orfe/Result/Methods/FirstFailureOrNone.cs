namespace Orfe;

public partial struct Result
{
    /// <summary>
    ///     Returns the first failure from the supplied <paramref name="results"/>.
    ///     If there is no failure, a success result is returned.
    /// </summary>
    public static Option<Result<T,TE>> FirstFailureOrNone<T,TE>(params Result<T,TE>[] results)
    {
        foreach (var result in results)
        {
            if (result.IsFailure)
                return result;
        }

        return Option<Result<T,TE>>.None;
    }
}

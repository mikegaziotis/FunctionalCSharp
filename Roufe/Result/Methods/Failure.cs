namespace Roufe;

public partial struct Result
{
    /// <summary>
    ///     Creates a failure result with the given error message.
    /// </summary>
    public static Result<Unit,string> Failure(string error)
    {
        return new(true, error, default);
    }

    /// <summary>
    ///     Creates a failure result with the given error message.
    /// </summary>
    public static Result<T, string> Failure<T>(string error)
    {
        return new (true, error, default);
    }

    /// <summary>
    ///     Creates a failure result with the given error.
    /// </summary>
    public static Result<T, TE> Failure<T, TE>(TE? error)
    {
        return new (true, error, default);
    }

    /// <summary>
    ///     Creates a failure result with the given error.
    /// </summary>
    public static Result<Unit, TE> Failure<TE>(TE error)
    {
        return new (true, error, default);
    }

    public static Result<Unit,Unit> Failure()
    {
        return new (true, default, default);
    }
}

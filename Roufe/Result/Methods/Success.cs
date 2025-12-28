namespace Roufe;

public partial struct Result
{

    /// <summary>
    ///     Creates a success result containing the given value.
    /// </summary>
    public static Result<T, TE> Success<T, TE>(T? value)
        => new(false, default, value);

    /// <summary>
    ///     Creates a success result containing the given value.
    /// </summary>
    public static Result<T, Unit> Success<T>(T? value)
        => new(false, default, value);

    /// <summary>
    ///     Creates a success result containing the given error.
    /// </summary>
    public static Result<Unit,TE> Success<TE>()
        => new(false, default, default);

    /// <summary>
    ///     Creates a success result containing the given error.
    /// </summary>
    public static Result<Unit,Unit> Success()
        => new(false, default, default);
}

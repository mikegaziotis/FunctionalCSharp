namespace Orfe;

public static partial class OptionExtensions
{
    /// <summary>
    /// Mark the possibly null value as optional. Null is not an error, it's just null.
    /// </summary>
    public static Result<Option<T>, TE> Optional<T, TE>(this Option<Result<T, TE>> option)
        => option switch
        {
            { HasNoValue: true } => Result.Success<Option<T>, TE>(Option<T>.None),
            { Value.IsFailure: true } => Result.Failure<Option<T>, TE>(option.Value.Error),
            _ => Result.Success<Option<T>, TE>(Option.From(option.Value.Value))
        };
}

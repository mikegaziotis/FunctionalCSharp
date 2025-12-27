namespace Orfe;

public static partial class OptionExtensions
{
    /// <summary>
    ///     Flattens nested <see cref="Option{T}" />s into a single <see cref="Option{T}" />.
    /// </summary>
    /// <returns>The flattened <see cref="Option{T}" />.</returns>
    public static Option<T> Flatten<T>(in this Option<Option<T>> option)
        => option.GetValueOrDefault();

}

using System;

namespace Roufe;

public static partial class OptionExtensions
{
    /// <summary>
    /// Convert an optional value into a Result. The result, being optional, is of type Option.
    /// </summary>
    public static Result<Option<TOut>, TError> BindOptional<TIn, TOut, TError>(this Option<TIn> option, Func<TIn, Result<TOut, TError>> bind)
        => option.Bind(v => Option.From(bind(v)))
            .Optional();
}

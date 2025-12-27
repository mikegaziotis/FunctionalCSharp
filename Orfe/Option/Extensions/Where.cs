using System;

namespace Orfe;

public static partial class OptionExtensions
{
    public static Option<T> Where<T>(in this Option<T> option, Func<T, bool> predicate)
    {
        if (option.HasNoValue)
            return Option<T>.None;

        return predicate(option.GetValueOrThrow())
            ? option
            : Option<T>.None;
    }
}

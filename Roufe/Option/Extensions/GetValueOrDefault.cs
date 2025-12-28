using System;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)
    {
        public T GetValueOrDefault(Func<T> defaultValue)
            => option.HasNoValue
                ? defaultValue()
                : option.GetValueOrThrow();

        public TK? GetValueOrDefault<TK>(Func<T, TK> selector, TK? defaultValue = default)
            => option.HasNoValue
                ? defaultValue
                : selector(option.GetValueOrThrow());

        public TK GetValueOrDefault<TK>(Func<T, TK> selector, Func<TK> defaultValue)
            => option.HasNoValue
                ? defaultValue()
                : selector(option.GetValueOrThrow());
    }
}

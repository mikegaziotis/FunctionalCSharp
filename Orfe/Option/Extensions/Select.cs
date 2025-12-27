using System;

namespace Orfe;

public static partial class OptionExtensions
{
    public static Option<TK> Select<T, TK>(in this Option<T> option, Func<T, TK> selector)
        => option.Map(selector);

}

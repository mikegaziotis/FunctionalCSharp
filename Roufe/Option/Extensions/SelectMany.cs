using System;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)
    {
        public Option<TK> SelectMany<TK>(Func<T, Option<TK>> selector)
            => option.Bind(selector);

        public Option<TV> SelectMany<TU, TV>(Func<T, Option<TU>> selector, Func<T, TU, TV> project)
            => option.GetValueOrDefault(
                x => selector(x).GetValueOrDefault(u => project(x, u), Option<TV>.None),
                Option<TV>.None);
    }
}

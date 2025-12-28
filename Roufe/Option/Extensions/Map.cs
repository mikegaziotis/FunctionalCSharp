using System;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)
    {
        public Option<TK> Map<TK>(Func<T, TK> selector)
            => option.HasNoValue
                ? Option<TK>.None
                : selector(option.GetValueOrThrow());


        public Option<TK> Map<TK, TContext>(Func<T, TContext, TK> selector, TContext context)
            => option.HasNoValue
                ? Option<TK>.None
                : selector(option.GetValueOrThrow(), context);

    }
}

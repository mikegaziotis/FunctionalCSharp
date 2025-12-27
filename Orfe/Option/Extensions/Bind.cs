using System;

namespace Orfe;

public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)
    {
        public Option<TK> Bind<TK>(Func<T, Option<TK>> selector)
        => option.HasNoValue
            ? Option<TK>.None :
            selector(option.GetValueOrThrow());


        public Option<TK> Bind<TK, TContext>(Func<T, TContext, Option<TK>> selector, TContext context)
        =>  option.HasNoValue
            ? Option<TK>.None :
            selector(option.GetValueOrThrow(), context);

    }
}

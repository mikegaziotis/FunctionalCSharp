using System;
using System.Collections.Generic;

namespace Orfe;

public static partial class OptionExtensions
{
    public static TE Match<TE, T>(in this Option<T> option, Func<T, TE> some, Func<TE> none)
    {
        return option.HasValue ? some(option.GetValueOrThrow()) : none();
    }

    public static TE Match<TE, T, TContext>(
        in this Option<T> option,
        Func<T, TContext, TE> some,
        Func<TContext, TE> none,
        TContext context)
    => option.HasValue
        ? some(option.GetValueOrThrow(), context)
        : none(context);

    extension<T>(in Option<T> option)
    {
        public void Match(Action<T> some, Action none)
        {
            if (option.HasValue)
                some(option.GetValueOrThrow());
            else
                none();
        }


        public void Match<TContext>(Action<T, TContext> some, Action<TContext> none, TContext context)
        {
            if (option.HasValue)
                some(option.GetValueOrThrow(), context);
            else
                none(context);
        }


    }

    public static TE Match<TE, TKey, TValue>(
        in this Option<KeyValuePair<TKey, TValue>> option,
        Func<TKey, TValue, TE> some,
        Func<TE> none)
    => option.HasValue
            ? some.Invoke(option.GetValueOrThrow().Key, option.GetValueOrThrow().Value)
            : none.Invoke();


    public static TE Match<TE, TKey, TValue, TContext>(
        in this Option<KeyValuePair<TKey, TValue>> option,
        Func<TKey, TValue, TContext, TE> some,
        Func<TContext, TE> none,
        TContext context)
    => option.HasValue
            ? some.Invoke(option.GetValueOrThrow().Key, option.GetValueOrThrow().Value, context)
            : none.Invoke(context);


    extension<TKey, TValue>(in Option<KeyValuePair<TKey, TValue>> option)
    {
        public void Match(Action<TKey, TValue> some, Action none)
        {
            if (option.HasValue)
                some.Invoke(option.GetValueOrThrow().Key, option.GetValueOrThrow().Value);
            else
                none.Invoke();
        }

        public void Match<TContext>(Action<TKey, TValue, TContext> some,
            Action<TContext> none,
            TContext context
        )
        {
            if (option.HasValue)
                some.Invoke(option.GetValueOrThrow().Key, option.GetValueOrThrow().Value, context);
            else
                none.Invoke(context);
        }
    }
}

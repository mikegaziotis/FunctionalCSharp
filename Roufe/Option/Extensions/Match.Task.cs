using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Roufe;

public static partial class OptionExtensions
{
    public static async Task<TE> Match<TE, T>(
        this Option<T> option,
        Func<T, CancellationToken, Task<TE>> some,
        Func<CancellationToken, Task<TE>> none,
        CancellationToken cancellationToken = default
    )
    {
        return option.HasValue
            ? await some(option.GetValueOrThrow(), cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none(cancellationToken).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async Task<TE> Match<TE, T, TContext>(
        this Option<T> option,
        Func<T, TContext, CancellationToken, Task<TE>> some,
        Func<TContext, CancellationToken, Task<TE>> none,
        TContext context,
        CancellationToken cancellationToken = default
    )
    {
        return option.HasValue
            ? await some(option.GetValueOrThrow(), context, cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
    }

    extension<T>(Option<T> option)
    {
        public async Task Match(Func<T, CancellationToken, Task> some,
            Func<CancellationToken, Task> none,
            CancellationToken cancellationToken = default)
        {
            if (option.HasValue)
                await some(option.GetValueOrThrow(), cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            else
                await none(cancellationToken).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task Match<TContext>(Func<T, TContext, CancellationToken, Task> some,
            Func<TContext, CancellationToken, Task> none,
            TContext context,
            CancellationToken cancellationToken = default)
        {
            if (option.HasValue)
                await some(option.GetValueOrThrow(), context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            else
                await none(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
        }
    }

    public static async Task<TE> Match<TE, TKey, TValue>(
        this Option<KeyValuePair<TKey, TValue>> option,
        Func<TKey, TValue, CancellationToken, Task<TE>> some,
        Func<CancellationToken, Task<TE>> none,
        CancellationToken cancellationToken = default)
    {
        return option.HasValue
            ? await some.Invoke(
                option.GetValueOrThrow().Key,
                option.GetValueOrThrow().Value,
                cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none.Invoke(cancellationToken).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async Task<TE> Match<TE, TKey, TValue, TContext>(
        this Option<KeyValuePair<TKey, TValue>> option,
        Func<TKey, TValue, TContext, CancellationToken, Task<TE>> some,
        Func<TContext, CancellationToken, Task<TE>> none,
        TContext context,
        CancellationToken cancellationToken = default)
    => option.HasValue
            ? await some.Invoke(
                option.GetValueOrThrow().Key,
                option.GetValueOrThrow().Value,
                context,
                cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none.Invoke(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);

    extension<TKey, TValue>(Option<KeyValuePair<TKey, TValue>> option)
    {
        public async Task Match(Func<TKey, TValue, CancellationToken, Task> some,
            Func<CancellationToken, Task> none,
            CancellationToken cancellationToken = default
        )
        {
            if (option.HasValue)
            {
                await some.Invoke(
                    option.GetValueOrThrow().Key,
                    option.GetValueOrThrow().Value,
                    cancellationToken
                ).ConfigureAwait(DefaultConfigureAwait);
            }
            else
            {
                await none.Invoke(cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            }
        }

        public async Task Match<TContext>(Func<TKey, TValue, TContext, CancellationToken, Task> some,
            Func<TContext, CancellationToken, Task> none,
            TContext context,
            CancellationToken cancellationToken = default)
        {
            if (option.HasValue)
            {
                await some.Invoke(
                    option.GetValueOrThrow().Key,
                    option.GetValueOrThrow().Value,
                    context,
                    cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            }
            else
            {
                await none.Invoke(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            }
        }
    }
}

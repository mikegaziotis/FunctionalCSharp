using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class OptionExtensions
{
    public static async ValueTask<TE> Match<TE, T>(
        this Option<T> option,
        Func<T, CancellationToken, ValueTask<TE>> some,
        Func<CancellationToken, ValueTask<TE>> none,
        CancellationToken cancellationToken = default)
    => option.HasValue
            ? await some(option.GetValueOrThrow(), cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none(cancellationToken).ConfigureAwait(DefaultConfigureAwait);


    public static async ValueTask<TE> Match<TE, T, TContext>(
        this Option<T> option,
        Func<T, TContext, CancellationToken, ValueTask<TE>> some,
        Func<TContext, CancellationToken, ValueTask<TE>> none,
        TContext context,
        CancellationToken cancellationToken = default)
    => option.HasValue
            ? await some(option.GetValueOrThrow(), context, cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);


    extension<T>(Option<T> option)
    {
        public async ValueTask Match(Func<T, CancellationToken, ValueTask> some,
            Func<CancellationToken, ValueTask> none,
            CancellationToken cancellationToken = default
        )
        {
            if (option.HasValue)
                await some(option.GetValueOrThrow(), cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            else
                await none(cancellationToken).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask Match<TContext>(Func<T, TContext, CancellationToken, ValueTask> some,
            Func<TContext, CancellationToken, ValueTask> none,
            TContext context,
            CancellationToken cancellationToken = default)
        {
            if (option.HasValue)
                await some(option.GetValueOrThrow(), context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            else
                await none(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
        }
    }

    public static async ValueTask<TE> Match<TE, TKey, TValue>(
        this Option<KeyValuePair<TKey, TValue>> option,
        Func<TKey, TValue, CancellationToken, ValueTask<TE>> some,
        Func<CancellationToken, ValueTask<TE>> none,
        CancellationToken cancellationToken = default)
    => option.HasValue
            ? await some.Invoke(
                option.GetValueOrThrow().Key,
                option.GetValueOrThrow().Value,
                cancellationToken).ConfigureAwait(DefaultConfigureAwait)
            : await none.Invoke(cancellationToken).ConfigureAwait(DefaultConfigureAwait);


    public static async ValueTask<TE> Match<TE, TKey, TValue, TContext>(
        this Option<KeyValuePair<TKey, TValue>> option,
        Func<TKey, TValue, TContext, CancellationToken, ValueTask<TE>> some,
        Func<TContext, CancellationToken, ValueTask<TE>> none,
        TContext context,
        CancellationToken cancellationToken = default)
    => option.HasValue
            ? await some.Invoke(
                option.GetValueOrThrow().Key,
                option.GetValueOrThrow().Value,
                context,
                cancellationToken
            ).ConfigureAwait(DefaultConfigureAwait)
            : await none.Invoke(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);


    extension<TKey, TValue>(Option<KeyValuePair<TKey, TValue>> option)
    {
        public async ValueTask Match(Func<TKey, TValue, CancellationToken, ValueTask> some,
            Func<CancellationToken, ValueTask> none,
            CancellationToken cancellationToken = default)
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

        public async ValueTask Match<TContext>(
            Func<TKey, TValue, TContext, CancellationToken, ValueTask> some,
            Func<TContext, CancellationToken, ValueTask> none,
            TContext context,
            CancellationToken cancellationToken = default
        )
        {
            if (option.HasValue)
            {
                await some.Invoke(
                    option.GetValueOrThrow().Key,
                    option.GetValueOrThrow().Value,
                    context,
                    cancellationToken
                ).ConfigureAwait(DefaultConfigureAwait);
            }
            else
            {
                await none.Invoke(context, cancellationToken).ConfigureAwait(DefaultConfigureAwait);
            }
        }
    }
}

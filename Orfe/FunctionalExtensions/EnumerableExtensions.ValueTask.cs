// ReSharper disable global PossibleMultipleEnumeration
// ReSharper disable MemberCanBePrivate.Global
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;


namespace Orfe;

public static partial class EnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static async Task<IEnumerable<T>> ParallelForEachAsync<T>(this IEnumerable<T> collection, Func<T,CancellationToken,ValueTask> func, ParallelOptions parallelOptions)
    {
        await Parallel.ForEachAsync(collection, parallelOptions, func).ConfigureAwait(DefaultConfigureAwait);
        return collection;
    }

    /*extension<T>(IEnumerable<T> collection)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private async ValueTask<IEnumerable<T>> IterateAsync(Func<T,ValueTask> action)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var item in collection)
                await action(item).ConfigureAwait(DefaultConfigureAwait);

            return collection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> TapAsync(Func<T,ValueTask> action)
            => await IterateAsync(collection, action).ConfigureAwait(DefaultConfigureAwait);


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> ForEachAsync(Func<T,ValueTask> action)
            => await IterateAsync(collection, action).ConfigureAwait(DefaultConfigureAwait);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> IterAsync(Func<T,ValueTask> action)
            => await IterateAsync(collection, action).ConfigureAwait(DefaultConfigureAwait);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<TK>> MapAsync<TK>(Func<T, ValueTask<TK>> func)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(func);

            List<TK> result = [];
            foreach (var item in collection)
            {
                result.Add(await func(item).ConfigureAwait(DefaultConfigureAwait));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> FilterAsync(Func<T, ValueTask<bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(predicate);

            List<T> result = [];
            foreach (var item in collection)
            {
                if (await predicate(item).ConfigureAwait(DefaultConfigureAwait))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<TK> FoldAsync<TK>( Func<TK, T, ValueTask<TK>> func, TK seed)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(func);

            var acc = seed;
            foreach (var item in collection)
            {
                acc = await func(acc, item);
            }
            return acc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<T> ReduceAsync(Func<T, T, ValueTask<T>> func)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(func);

            using var e = collection.GetEnumerator();
            if (!e.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");

            var acc = e.Current;
            while (e.MoveNext())
            {
                acc = await func(acc, e.Current).ConfigureAwait(DefaultConfigureAwait);
            }
            return acc;
        }
    }

    extension<T>(ValueTask<IEnumerable<T>> awaitedCollectionTask)
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> ForEachAsync(Func<T,ValueTask> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.ForEachAsync(action).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> IterAsync(Func<T,ValueTask> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.IterAsync(action).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<TK>> MapAsync<TK>(Func<T, ValueTask<TK>> func)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.MapAsync(func).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<IEnumerable<T>> FilterAsync(Func<T, ValueTask<bool>> predicate)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.FilterAsync(predicate).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<TK> FoldAsync<TK>(Func<TK, T, ValueTask<TK>> func, TK seed)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.FoldAsync(func, seed).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async ValueTask<T> ReduceAsync(Func<T, T, ValueTask<T>> func)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.ReduceAsync(func).ConfigureAwait(DefaultConfigureAwait);
        }

    }*/
}

// ReSharper disable global PossibleMultipleEnumeration
// ReSharper disable MemberCanBePrivate.Global
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Orfe;

public static partial class EnumerableExtensions
{
    extension<T>(IEnumerable<T> collection)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private async Task<IEnumerable<T>> IterateAsync(Func<T,Task> action)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var item in collection)
                await action(item).ConfigureAwait(DefaultConfigureAwait);

            return collection;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> TapAsync(Func<T,Task> action)
            => await IterateAsync(collection, action).ConfigureAwait(DefaultConfigureAwait);


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> ForEachAsync(Func<T,Task> action)
            => await IterateAsync(collection, action).ConfigureAwait(DefaultConfigureAwait);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> IterAsync(Func<T,Task> action)
            => await IterateAsync(collection, action).ConfigureAwait(DefaultConfigureAwait);

        public async Task<IEnumerable<TK>> MapAsync<TK>(Func<T, Task<TK>> func)
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

        public async Task<IEnumerable<T>> FilterAsync(Func<T, Task<bool>> predicate)
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
        public async Task<TK> FoldAsync<TK>( Func<TK, T, Task<TK>> func, TK seed)
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
    }

    extension<T>(Task<IEnumerable<T>> awaitedCollectionTask)
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> ForEachAsync(Func<T,Task> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.IterateAsync(action).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> TapAsync(Func<T,Task> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.IterateAsync(action).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> IterAsync(Func<T,Task> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.IterateAsync(action).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<TK>> MapAsync<TK>(Func<T, Task<TK>> func)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.MapAsync(func).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> FilterAsync(Func<T, Task<bool>> predicate)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.FilterAsync(predicate).ConfigureAwait(DefaultConfigureAwait);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<TK> FoldAsync<TK>(Func<TK, T, Task<TK>> func, TK seed)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return await collection.FoldAsync(func, seed).ConfigureAwait(DefaultConfigureAwait);
        }

    }
}

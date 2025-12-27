// ReSharper disable global PossibleMultipleEnumeration
// ReSharper disable MemberCanBePrivate.Global
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace Orfe;

public static partial class EnumerableExtensions
{
    extension<T>(IEnumerable<T> collection)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public T[] ToSafeArray()
        {
            ArgumentNullException.ThrowIfNull(collection);
            return collection as T[] ?? collection.ToArray();
        }

        /// <summary>
        /// Executes the given action for each item in the collection and returns the original collection.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private IEnumerable<T> Iterate(Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(action);
            var items = collection.ToSafeArray();
            foreach (var item in items)
            {
                action(item);
            }
            return items;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public IEnumerable<T> Tap(Action<T> action)
            => Iterate(collection, action);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public IEnumerable<T> ForEach(Action<T> action)
            => Iterate(collection, action);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public IEnumerable<T> Iter(Action<T> action)
            => Iterate(collection, action);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public IEnumerable<TK> Map<TK>(Func<T, TK> func)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(func);
            return collection.Select(func);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(predicate);
            return collection.Where(predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public TK Fold<TK>(Func<TK, T, TK> func, TK seed)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(func);
            ArgumentNullException.ThrowIfNull(seed);
            return collection.Aggregate(seed, func);
        }

    }

    extension<T>(Task<IEnumerable<T>> awaitedCollectionTask)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<T[]> ToSafeArrayAsync()
        {
            ArgumentNullException.ThrowIfNull(awaitedCollectionTask);
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection as T[] ?? collection.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> Tap(Action<T> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection.Iterate(action);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> ForEach(Action<T> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection.Iterate(action);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> Iter(Action<T> action)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection.Iterate(action);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<TK>> Map<TK>(Func<T, TK> func)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection.Select(func);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<IEnumerable<T>> Filter(Func<T, bool> predicate)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection.Filter(predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<TK> Fold<TK>(Func<TK, T, TK> func, TK seed)
        {
            var collection = await awaitedCollectionTask.ConfigureAwait(DefaultConfigureAwait);
            return collection.Fold(func, seed);
        }

    }
}

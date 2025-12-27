using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Orfe;

public static class GenericExtensions
{
    extension<T>(T value) where T: IEquatable<T>
    {
        public bool IsIn(params T[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            return Enumerable.Contains(values, value);
        }
        public bool IsIn(IEnumerable<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            return values.Contains(value);
        }

    }

    extension<T>(T value)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public TResult Pipe<TResult>(Func<T, TResult> func)
        {
            ArgumentNullException.ThrowIfNull(func);
            return func(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<TResult> PipeAsync<TResult>(Func<T, Task<TResult>> func)
        {
            ArgumentNullException.ThrowIfNull(func);
            return await func(value).ConfigureAwait(DefaultConfigureAwait);
        }

    }

    extension<T>(Task<T> task)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<TResult> Pipe<TResult>(Func<T,TResult> func)
        {
            ArgumentNullException.ThrowIfNull(func);
            var t = await task.ConfigureAwait(DefaultConfigureAwait);
            return func(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public async Task<TResult> PipeAsync<TResult>(Func<T, Task<TResult>> func)
        {
            ArgumentNullException.ThrowIfNull(func);
            var t = await task.ConfigureAwait(DefaultConfigureAwait);
            return await func(t).ConfigureAwait(DefaultConfigureAwait);
        }
    }
}

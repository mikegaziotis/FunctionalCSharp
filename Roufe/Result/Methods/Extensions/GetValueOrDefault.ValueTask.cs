using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks
{
    public static partial class ResultExtensions
    {
        extension<T,TE>(ValueTask<Result<T,TE>> resultTask)
        {
            public async ValueTask<T> GetValueOrDefault(Func<ValueTask<T>> defaultValue)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.GetValueOrDefault(defaultValue).ConfigureAwait(DefaultConfigureAwait);
            }

            public async ValueTask<TK?> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> selector, TK? defaultValue = default)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
            }

            public async ValueTask<K> GetValueOrDefault<K>(Func<T, ValueTask<K>> selector, Func<ValueTask<K>> defaultValue)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
            }

            public async ValueTask<T> GetValueOrDefault(Func<T> defaultValue)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.GetValueOrDefault(defaultValue);
            }

            public async ValueTask<TK?> GetValueOrDefault<TK>(Func<T, TK> selector, TK? defaultValue = default)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.GetValueOrDefault(selector, defaultValue);
            }

            public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, TK> selector, Func<TK> defaultValue)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.GetValueOrDefault(selector, defaultValue);
            }
        }

        extension<T,TE>(Result<T,TE> result)
        {
            public async ValueTask<T> GetValueOrDefault(Func<ValueTask<T>> valueTask)
            {
                if (result.IsFailure)
                    return await valueTask().ConfigureAwait(DefaultConfigureAwait);

                return result.Value;
            }

            public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, TK> selector, Func<ValueTask<TK>> valueTask)
            {
                if (result.IsFailure)
                    return await valueTask().ConfigureAwait(DefaultConfigureAwait);

                return selector(result.Value);
            }

            public async ValueTask<TK?> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> valueTask, TK? defaultValue = default)
            {
                if (result.IsFailure)
                    return defaultValue;

                return await valueTask(result.Value).ConfigureAwait(DefaultConfigureAwait);
            }

            public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> valueTask, Func<ValueTask<TK>> defaultValue)
            {
                if (result.IsFailure)
                    return await defaultValue().ConfigureAwait(DefaultConfigureAwait);

                return await valueTask(result.Value).ConfigureAwait(DefaultConfigureAwait);
            }
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T,TE>(Task<Result<T,TE>> resultTask)
    {
        public async Task<T> GetValueOrDefault(Func<Task<T>> defaultValue)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.GetValueOrDefault(defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<TK?> GetValueOrDefault<TK>(Func<T, Task<TK>> selector, TK? defaultValue = default)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<TK> GetValueOrDefault<TK>(Func<T, Task<TK>> selector, Func<Task<TK>> defaultValue)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<T> GetValueOrDefault(Func<T> defaultValue)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.GetValueOrDefault(defaultValue);
        }

        public async Task<TK?> GetValueOrDefault<TK>(Func<T, TK> selector, TK? defaultValue = default)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.GetValueOrDefault(selector, defaultValue);
        }

        public async Task<TK> GetValueOrDefault<TK>(Func<T, TK> selector,
            Func<TK> defaultValue)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.GetValueOrDefault(selector, defaultValue);
        }
    }

    extension<T,TE>(Result<T,TE> result)
    {
        public async Task<T> GetValueOrDefault(Func<Task<T>> defaultValue)
            => result.IsFailure
                ? await defaultValue().ConfigureAwait(DefaultConfigureAwait)
                : result.Value;


        public async Task<TK> GetValueOrDefault<TK>(Func<T, TK> selector, Func<Task<TK>> defaultValue)
            => result.IsFailure
                ? await defaultValue().ConfigureAwait(DefaultConfigureAwait)
                : selector(result.Value);

        public async Task<TK?> GetValueOrDefault<TK>(Func<T, Task<TK>> selector, TK? defaultValue = default)
            => result.IsFailure
                ? defaultValue
                : await selector(result.Value).ConfigureAwait(DefaultConfigureAwait);


        public async Task<TK> GetValueOrDefault<TK>(Func<T, Task<TK>> selector, Func<Task<TK>> defaultValue)
            => result.IsFailure
                ? await defaultValue().ConfigureAwait(DefaultConfigureAwait)
                : await selector(result.Value).ConfigureAwait(DefaultConfigureAwait);

    }
}

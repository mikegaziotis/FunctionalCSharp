using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        public async Task<T> GetValueOrDefault(Func<Task<T>> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.GetValueOrDefault(defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<TK?> GetValueOrDefault<TK>(Func<T, Task<TK>> selector, TK? defaultValue = default)
        {
            ArgumentNullException.ThrowIfNull(selector);
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<TK> GetValueOrDefault<TK>(Func<T, Task<TK>> selector, Func<Task<TK>> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<T> GetValueOrDefault(Func<T> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrDefault(defaultValue);
        }

        public async Task<TK?> GetValueOrDefault<TK>(Func<T, TK> selector,
            TK? defaultValue = default)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrDefault(selector, defaultValue);
        }

        public async Task<TK> GetValueOrDefault<TK>(Func<T, TK> selector,
            Func<TK> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrDefault(selector, defaultValue);
        }
    }

    extension<T>(Option<T> option)
    {
        public async Task<T> GetValueOrDefault(Func<Task<T>> defaultValue)
        {
            if (option.HasNoValue)
                return await defaultValue().ConfigureAwait(DefaultConfigureAwait);

            return option.GetValueOrThrow();
        }

        public async Task<TK> GetValueOrDefault<TK>(Func<T, TK> selector,
            Func<Task<TK>> defaultValue)
        {
            if (option.HasNoValue)
                return await defaultValue().ConfigureAwait(DefaultConfigureAwait);

            return selector(option.GetValueOrThrow());
        }

        public async Task<TK?> GetValueOrDefault<TK>(Func<T, Task<TK>> selector,
            TK? defaultValue = default)
        {
            if (option.HasNoValue)
                return defaultValue;

            return await selector(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<TK> GetValueOrDefault<TK>(Func<T, Task<TK>> selector,
            Func<Task<TK>> defaultValue)
        {
            if (option.HasNoValue)
                return await defaultValue().ConfigureAwait(DefaultConfigureAwait);

            return await selector(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }
    }
}

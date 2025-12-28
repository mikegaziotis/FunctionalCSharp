using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class OptionExtensions
{
    extension<T>(ValueTask<Option<T>> optionTask)
    {
        public async ValueTask<T> GetValueOrDefault(Func<ValueTask<T>> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.GetValueOrDefault(defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<TK?> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> selector,
            TK? defaultValue = default)
        {
            ArgumentNullException.ThrowIfNull(selector);
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> selector,
            Func<ValueTask<TK>> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.GetValueOrDefault(selector, defaultValue).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<T> GetValueOrDefault(Func<T> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrDefault(defaultValue);
        }

        public async ValueTask<TK?> GetValueOrDefault<TK>(Func<T, TK> selector,
            TK? defaultValue = default)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrDefault(selector, defaultValue);
        }

        public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, TK> selector,
            Func<TK> defaultValue)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrDefault(selector, defaultValue);
        }
    }

    extension<T>(Option<T> option)
    {
        public async ValueTask<T> GetValueOrDefault(Func<ValueTask<T>> valueTask)
        {
            if (option.HasNoValue)
                return await valueTask().ConfigureAwait(DefaultConfigureAwait);

            return option.GetValueOrThrow();
        }

        public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, TK> selector, Func<ValueTask<TK>> valueTask)
        {
            if (option.HasNoValue)
                return await valueTask().ConfigureAwait(DefaultConfigureAwait);

            return selector(option.GetValueOrThrow());
        }

        public async ValueTask<TK?> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> valueTask, TK? defaultValue = default)
        {
            if (option.HasNoValue)
                return defaultValue;

            return await valueTask(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<TK> GetValueOrDefault<TK>(Func<T, ValueTask<TK>> valueTask, Func<ValueTask<TK>> defaultValue)
        {
            if (option.HasNoValue)
                return await defaultValue().ConfigureAwait(DefaultConfigureAwait);

            return await valueTask(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }
    }
}

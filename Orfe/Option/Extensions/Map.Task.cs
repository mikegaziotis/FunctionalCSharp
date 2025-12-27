using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        public async Task<Option<TK>> Map<TK>(Func<T, Task<TK>> selector)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Map(selector).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Option<TK>> Map<TK, TContext>(Func<T, TContext, Task<TK>> selector, TContext context)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Map(selector, context).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Option<TK>> Map<TK>(Func<T, TK> selector)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Map(selector);
        }

        public async Task<Option<TK>> Map<TK, TContext>(Func<T, TContext, TK> selector, TContext context)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Map(selector, context);
        }
    }

    extension<T>(Option<T> option)
    {
        public async Task<Option<TK>> Map<TK>(Func<T, Task<TK>> selector)
            => option.HasNoValue
                ? Option<TK>.None
                : await selector(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);

        public async Task<Option<TK>> Map<TK, TContext>(Func<T, TContext, Task<TK>> selector, TContext context)
            => option.HasNoValue
                ? Option<TK>.None
                : await selector(option.GetValueOrThrow(), context).ConfigureAwait(DefaultConfigureAwait);

    }
}

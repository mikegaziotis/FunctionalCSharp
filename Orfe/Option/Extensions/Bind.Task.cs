using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        public async Task<Option<TK>> Bind<TK>(Func<T, Task<Option<TK>>> selector)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Bind(selector).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Option<TK>> Bind<TK, TContext>(Func<T, TContext, Task<Option<TK>>> selector, TContext context)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Bind(selector, context).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Option<TK>> Bind<TK>(Func<T, Option<TK>> selector)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Bind(selector);
        }

        public async Task<Option<TK>> Bind<TK, TContext>(Func<T, TContext, Option<TK>> selector, TContext context)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Bind(selector, context);
        }
    }

    extension<T>(Option<T> option)
    {
        public Task<Option<TK>> Bind<TK>(Func<T, Task<Option<TK>>> selector)
            => option.HasNoValue
                ? Option<TK>.None.AsCompletedTask()
                : selector(option.GetValueOrThrow());


        public Task<Option<TK>> Bind<TK, TContext>(Func<T, TContext, Task<Option<TK>>> selector, TContext context)
            => option.HasNoValue
                ? Option<TK>.None.AsCompletedTask()
                : selector(option.GetValueOrThrow(), context);

    }
}

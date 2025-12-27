using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class OptionExtensions
{
    extension<T>(ValueTask<Option<T>> optionValueTask)
    {
        public async ValueTask<Option<TK>> Map<TK>(Func<T, ValueTask<TK>> valueTask
        )
        {
            var option = await optionValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Map(valueTask).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Option<TK>> Map<TK, TContext>(Func<T, TContext, ValueTask<TK>> valueTask,
            TContext context
        )
        {
            var option = await optionValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Map(valueTask, context).ConfigureAwait(DefaultConfigureAwait);
        }
        public async ValueTask<Option<TK>> Map<TK>(Func<T, TK> selector)
        {
            var option = await optionValueTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Map(selector);
        }

        public async ValueTask<Option<TK>> Map<TK, TContext>(Func<T, TContext, TK> selector, TContext context)
        {
            var option = await optionValueTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Map(selector, context);
        }
    }

    extension<T>(Option<T> option)
    {
        public async ValueTask<Option<TK>> Map<TK>(Func<T, ValueTask<TK>> valueTask)
        {
            if (option.HasNoValue)
                return Option<TK>.None;

            return await valueTask(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Option<TK>> Map<TK, TContext>(
            Func<T, TContext, ValueTask<TK>> valueTask,
            TContext context)
        {
            if (option.HasNoValue)
                return Option<TK>.None;

            return await valueTask(option.GetValueOrThrow(), context).ConfigureAwait(DefaultConfigureAwait);
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class OptionExtensions
{
    extension<T>(ValueTask<Option<T>> optionTask)
    {
        public async ValueTask<Option<TK>> Bind<TK>(Func<T, ValueTask<Option<TK>>> selector)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Bind(selector).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Option<TK>> Bind<TK, TContext>(Func<T, TContext, ValueTask<Option<TK>>> selector,
            TContext context)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Bind(selector, context).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Option<TK>> Bind<TK>(Func<T, Option<TK>> selector)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Bind(selector);
        }

        public async ValueTask<Option<TK>> Bind<TK, TContext>(Func<T, TContext, Option<TK>> selector,
            TContext context)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Bind(selector, context);
        }
    }

    extension<T>(Option<T> option)
    {
        public ValueTask<Option<TK>> Bind<TK>(Func<T, ValueTask<Option<TK>>> selector)
            => option.HasNoValue
                ? Option<TK>.None.AsCompletedValueTask()
                : selector(option.GetValueOrThrow());

        public ValueTask<Option<TK>> Bind<TK, TContext>(Func<T, TContext, ValueTask<Option<TK>>> selector,
            TContext context)
            => option.HasNoValue
                ? Option<TK>.None.AsCompletedValueTask()
                : selector(option.GetValueOrThrow(), context);

    }
}

using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class OptionExtensions
{
    extension<T>(ValueTask<Option<T>> optionTask)
    {
        public async ValueTask<Option<T>> Where(Func<T, ValueTask<bool>> predicate)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Where(predicate).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Option<T>> Where(Func<T, bool> predicate)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Where(predicate);
        }
    }

    public static async ValueTask<Option<T>> Where<T>(this Option<T> option, Func<T, ValueTask<bool>> predicate)
    {
        if (option.HasNoValue)
            return Option<T>.None;

        if (await predicate(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait))
            return option;

        return Option<T>.None;
    }
}

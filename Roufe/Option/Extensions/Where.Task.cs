using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        public async Task<Option<T>> Where(Func<T, Task<bool>> predicate)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return await option.Where(predicate).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Option<T>> Where(Func<T, bool> predicate)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Where(predicate);
        }
    }

    public static async Task<Option<T>> Where<T>(this Option<T> option, Func<T, Task<bool>> predicate)
    {
        if (option.HasNoValue)
            return Option<T>.None;

        if (await predicate(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait))
            return option;

        return Option<T>.None;
    }
}

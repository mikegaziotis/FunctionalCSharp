using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class OptionExtensions
{
    /// <param name="optionTask"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(Task<Option<T>> optionTask)
    {
        /// <summary>
        ///     Executes the given <paramref name="asyncAction" /> if the <paramref name="optionTask" /> produces no value
        /// </summary>
        /// <param name="asyncAction"></param>
        public async Task ExecuteNoValue(Func<Task> asyncAction)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasValue)
                return;

            await asyncAction().ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Executes the given <paramref name="action" /> if the <paramref name="optionTask" /> produces no value
        /// </summary>
        /// <param name="action"></param>
        public async Task ExecuteNoValue(Action action)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasValue)
                return;

            action();
        }
    }

    /// <summary>
    ///     Executes the given async <paramref name="action" /> if the <paramref name="option" /> has no value
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task ExecuteNoValue<T>(this Option<T> option, Func<Task> action)
    {
        if (option.HasValue)
            return;

        await action().ConfigureAwait(DefaultConfigureAwait);
    }
}

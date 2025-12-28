using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class OptionExtensions
{
    /// <param name="optionTask"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(ValueTask<Option<T>> optionTask)
    {
        /// <summary>
        /// Executes the given <paramref name="valueTask" /> if the <paramref name="optionTask" /> produces no value
        /// </summary>
        /// <param name="valueTask"></param>
        public async Task ExecuteNoValue(Func<ValueTask> valueTask)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasValue)
                return;

            await valueTask().ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        /// Executes the given <paramref name="action" /> if the <paramref name="optionTask" /> produces no value
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
    ///     Executes the given async <paramref name="valueTask" /> if the <paramref name="option" /> has no value
    /// </summary>
    /// <param name="option"></param>
    /// <param name="valueTask"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task ExecuteNoValue<T>(this Option<T> option, Func<ValueTask> valueTask)
    {
        if (option.HasValue)
            return;

        await valueTask().ConfigureAwait(DefaultConfigureAwait);
    }
}

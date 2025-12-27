using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class OptionExtensions
{
    /// <param name="optionTask"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(ValueTask<Option<T>> optionTask)
    {
        /// <summary>
        /// Executes the given <paramref name="valueTask" /> if the <paramref name="optionTask" /> produces a value
        /// </summary>
        /// <param name="valueTask"></param>
        /// <returns></returns>
        public async Task Execute(Func<T, ValueTask> valueTask)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasNoValue)
                return;

            await valueTask(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        /// Executes the given <paramref name="action" /> if the <paramref name="optionTask" /> produces a value
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task Execute(Action<T> action)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasNoValue)
                return;

            action(option.GetValueOrThrow());
        }
    }

    /// <summary>
    ///     Executes the given async <paramref name="valueTask" /> if the <paramref name="option" /> has a value
    /// </summary>
    /// <param name="option"></param>
    /// <param name="valueTask"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task Execute<T>(this Option<T> option, Func<T, ValueTask> valueTask)
    {
        if (option.HasNoValue)
            return;

        await valueTask(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
    }
}

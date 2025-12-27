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
        ///     Executes the given <paramref name="asyncAction" /> if the <paramref name="optionTask" /> produces a value
        /// </summary>
        /// <param name="asyncAction"></param>
        /// <returns></returns>
        public async Task Execute(Func<T, Task> asyncAction)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasNoValue)
                return;

            await asyncAction(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Executes the given <paramref name="action" /> if the <paramref name="optionTask" /> produces a value
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
    ///     Executes the given async <paramref name="action" /> if the <paramref name="option" /> has a value
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task Execute<T>(this Option<T> option, Func<T, Task> action)
    {
        if (option.HasNoValue)
            return;

        await action(option.GetValueOrThrow()).ConfigureAwait(DefaultConfigureAwait);
    }
}

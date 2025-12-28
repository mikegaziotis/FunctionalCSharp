using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class OptionExtensions
{
    extension<T>(ValueTask<Option<T>> optionTask)
    {
        public async ValueTask<T> GetValueOrThrow()
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrThrow();
        }

        /// <summary>
        ///     Returns <paramref name="optionTask" />'s inner value if it has one, otherwise throws an InvalidOperationException
        ///     with <paramref name="errorMessage" />
        /// </summary>
        /// <exception cref="InvalidOperationException">Option has no value.</exception>
        public async ValueTask<T> GetValueOrThrow(string errorMessage)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrThrow(errorMessage);
        }
    }
}

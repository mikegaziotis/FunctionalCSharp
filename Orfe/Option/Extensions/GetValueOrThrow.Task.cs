using System.Threading.Tasks;

namespace Orfe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        public async Task<T> GetValueOrThrow()
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrThrow();
        }

        /// <summary>
        ///     Returns <paramref name="optionTask" />'s inner value if it has one, otherwise throws an InvalidOperationException
        ///     with <paramref name="errorMessage" />
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Option has no value.</exception>
        public async Task<T> GetValueOrThrow(string errorMessage)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.GetValueOrThrow(errorMessage);
        }
    }
}

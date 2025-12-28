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
        /// Creates a new <see cref="Option{T}" /> if <paramref name="optionTask" /> is empty, using the supplied <paramref name="fallback" />, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(ValueTask<T> fallback)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasNoValue)
            {
                var value = await fallback.ConfigureAwait(DefaultConfigureAwait);
                return Option<T>.From(value);
            }

            return option;
        }

        /// <summary>
        /// Creates a new <see cref="Option{T}" /> if <paramref name="optionTask" /> is empty, using the result of the supplied <paramref name="fallbackOperation" />, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallbackOperation"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Func<ValueTask<T>> fallbackOperation)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasNoValue)
            {
                var value = await fallbackOperation().ConfigureAwait(DefaultConfigureAwait);

                return Option<T>.From(value);
            }

            return option;
        }

        /// <summary>
        /// Returns <paramref name="fallbackOperation" /> if <paramref name="optionTask" /> is empty, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallbackOperation"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Func<ValueTask<Option<T>>> fallbackOperation)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            if (option.HasNoValue)
                return await fallbackOperation().ConfigureAwait(DefaultConfigureAwait);

            return option;
        }

        /// <summary>
        /// Creates a new <see cref="Option{T}" /> if <paramref name="optionTask" /> is empty, using the supplied <paramref name="fallback" />, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(T fallback)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            return option.HasNoValue
                ? Option<T>.From(fallback)
                : option;
        }

        /// <summary>
        /// Creates a new <see cref="Option{T}" /> if <paramref name="optionTask" /> is empty, using the result of the supplied <paramref name="fallbackOperation" />, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallbackOperation"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Func<T> fallbackOperation)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            return option.HasNoValue
                ? Option<T>.From(fallbackOperation())
                : option;
        }

        /// <summary>
        /// Returns <paramref name="fallback" /> if <paramref name="optionTask" /> is empty, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Option<T> fallback)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            return option.HasNoValue
                ? fallback
                : option;
        }

        /// <summary>
        /// Returns <paramref name="fallbackOperation" /> if <paramref name="optionTask" /> is empty, otherwise it returns <paramref name="optionTask" />
        /// </summary>
        /// <param name="fallbackOperation"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Func<Option<T>> fallbackOperation)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            return option.HasNoValue ? fallbackOperation() : option;
        }
    }

    /// <param name="option"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Creates a new <see cref="Option{T}" /> if <paramref name="option" /> is empty, using the result of the supplied <paramref name="valueTaskFallbackOperation" />, otherwise it returns <paramref name="option" />
        /// </summary>
        /// <param name="valueTaskFallbackOperation"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Func<ValueTask<T>> valueTaskFallbackOperation)
        {
            if (option.HasNoValue)
                return await valueTaskFallbackOperation().ConfigureAwait(DefaultConfigureAwait);

            return option;
        }

        /// <summary>
        /// Returns <paramref name="valueTaskFallback" /> if <paramref name="option" /> is empty, otherwise it returns <paramref name="option" />
        /// </summary>
        /// <param name="valueTaskFallback"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(ValueTask<Option<T>> valueTaskFallback)
        {
            if (option.HasNoValue)
                return await valueTaskFallback.ConfigureAwait(DefaultConfigureAwait);

            return option;
        }

        /// <summary>
        /// Returns the result of <paramref name="valueTaskFallbackOperation" /> if <paramref name="option" /> is empty, otherwise it returns <paramref name="option" />
        /// </summary>
        /// <param name="valueTaskFallbackOperation"></param>
        /// <returns></returns>
        public async ValueTask<Option<T>> Or(Func<ValueTask<Option<T>>> valueTaskFallbackOperation)
        {
            if (option.HasNoValue)
                return await valueTaskFallbackOperation().ConfigureAwait(DefaultConfigureAwait);

            return option;
        }
    }
}

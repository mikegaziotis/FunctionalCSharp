using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapError(Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
            {
                await func().ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapError(Func<TE, Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
            {
                await func(result.Error).ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapError(Action action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapError(action);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapError(Action<TE> action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapError(action);
        }
    }


    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapError(Func<Task> func)
        {
            if (result.IsFailure)
            {
                await func().ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapError(Func<TE, Task> func)
        {
            if (result.IsFailure)
            {
                await func(result.Error).ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }
    }
}

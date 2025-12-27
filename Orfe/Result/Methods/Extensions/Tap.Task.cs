using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> Tap(Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess)
                await func().ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> Tap(Func<T, Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess)
                await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> Tap(Action action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Tap(action);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> Tap(Action<T> action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Tap(action);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> Tap(Func<Task> func)
        {
            if (result.IsSuccess)
                await func().ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> Tap(Func<T, Task> func)
        {
            if (result.IsSuccess)
                await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapError(Func<ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
            {
                await func().ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapError(Func<TE, ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
            {
                await func(result.Error).ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapError(Action action)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapError(action);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapError(Action<TE> action)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapError(action);
        }
    }


    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapError(Func<ValueTask> func)
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
        public async ValueTask<Result<T, TE>> TapError(Func<TE, ValueTask> func)
        {
            if (result.IsFailure)
            {
                await func(result.Error).ConfigureAwait(DefaultConfigureAwait);
            }

            return result;
        }
    }
}

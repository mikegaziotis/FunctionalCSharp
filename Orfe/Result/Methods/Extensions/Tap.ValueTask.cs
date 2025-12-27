using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Tap(Func<ValueTask> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess)
                await valueTask().ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Tap(Func<T, ValueTask> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess)
                await valueTask(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Tap(Action action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Tap(action);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Tap(Action<T> action)
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
        public async ValueTask<Result<T, TE>> Tap(Func<ValueTask> valueTask)
        {
            if (result.IsSuccess)
                await valueTask().ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Tap(Func<T, ValueTask> valueTask)
        {
            if (result.IsSuccess)
                await valueTask(result.Value).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(bool condition, Func<Task> func)
            => condition
                ? resultTask.TapError(func)
                : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(bool condition, Func<TE, Task> func)
            => condition
                ? resultTask.TapError(func)
                : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return await result.TapError(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<TE, Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return await result.TapError(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(bool condition, Action action)
            => condition
                ? resultTask.TapError(action)
                : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(bool condition, Action<TE> action)
            => condition
                ? resultTask.TapError(action)
                : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Action action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return result.TapError(action);

            return result;
        }


        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Action<TE> action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return result.TapError(action);

            return result;
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(bool condition, Func<Task> func)
            => condition
                ? result.TapError(func)
                : Task.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(bool condition, Func<TE, Task> func)
            => condition
                ? result.TapError(func)
                : Task.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<Task> func)
            => result.IsFailure && predicate(result.Error)
                ? result.TapError(func)
                : Task.FromResult(result);


        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<TE, Task> func)
            => result.IsFailure && predicate(result.Error)
                ? result.TapError(func)
                : Task.FromResult(result);

    }
}

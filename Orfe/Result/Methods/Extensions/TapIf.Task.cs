using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(bool condition, Func<Task> func)
            => condition
                ? resultTask.Tap(func)
                : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(bool condition, Func<T, Task> func)
            => condition
                ? resultTask.Tap(func)
                : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && predicate(result.Value))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<T, Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && predicate(result.Value))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapIf(Func<Task<bool>> predicate, Func<T, Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && await predicate().ConfigureAwait(DefaultConfigureAwait))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapIf(Func<T, Task<bool>> predicate, Func<T, Task> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(bool condition, Action action)
            => condition ? resultTask.Tap(action) : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(bool condition, Action<T> action)
            => condition ? resultTask.Tap(action) : resultTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapIf(Func<T, bool> predicate, Action action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsSuccess && predicate(result.Value)
                ? result.Tap(action)
                : result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapIf(Func<T, bool> predicate, Action<T> action)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsSuccess && predicate(result.Value)
                ? result.Tap(action)
                : result;
        }
    }


    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(bool condition, Func<Task> func)
            => condition
                ? result.Tap(func)
                : Task.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(bool condition, Func<T, Task> func)
            => condition ? result.Tap(func) : Task.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<Task> func)
            => result.IsSuccess && predicate(result.Value)
                ? result.Tap(func)
                : Task.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Task<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<T, Task> func)
        {
            if (result.IsSuccess && predicate(result.Value))
                return result.Tap(func);
            else
                return Task.FromResult(result);
        }
    }
}

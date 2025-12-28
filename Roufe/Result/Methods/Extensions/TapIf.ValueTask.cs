using System;
using System.Threading.Tasks;
using Roufe.ValueTasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{

    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(bool condition, Func<ValueTask> func)
            => condition
                ? resultValueTask.Tap(func)
                : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(bool condition, Func<T, ValueTask> func)
            => condition
                ? resultValueTask.Tap(func)
                : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && predicate(result.Value))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<T, ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && predicate(result.Value))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapIf(Func<ValueTask<bool>> predicate, Func<T, ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && await predicate().ConfigureAwait(DefaultConfigureAwait))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapIf(Func<T, ValueTask<bool>> predicate, Func<T, ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return await result.Tap(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(bool condition, Action action)
            => condition ? resultValueTask.Tap(action) : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(bool condition, Action<T> action)
            => condition ? resultValueTask.Tap(action) : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapIf(Func<T, bool> predicate, Action action)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsSuccess && predicate(result.Value)
                ? result.Tap(action)
                : result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapIf(Func<T, bool> predicate, Action<T> action)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

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
        public ValueTask<Result<T, TE>> TapIf(bool condition, Func<ValueTask> func)
            => condition
                ? result.Tap(func)
                : ValueTask.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(bool condition, Func<T, ValueTask> func)
            => condition ? result.Tap(func) : ValueTask.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<ValueTask> func)
            => result.IsSuccess && predicate(result.Value)
                ? result.Tap(func)
                : ValueTask.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapIf(Func<T, bool> predicate, Func<T, ValueTask> func)
        {
            if (result.IsSuccess && predicate(result.Value))
                return result.Tap(func);
            else
                return ValueTask.FromResult(result);
        }
    }
}

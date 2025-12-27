using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(bool condition, Func<ValueTask> func)
            => condition
                ? resultValueTask.TapError(func)
                : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(bool condition, Func<TE, ValueTask> func)
            => condition
                ? resultValueTask.TapError(func)
                : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return await result.TapError(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<TE, ValueTask> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return await result.TapError(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(bool condition, Action action)
            => condition
                ? resultValueTask.TapError(action)
                : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(bool condition, Action<TE> action)
            => condition
                ? resultValueTask.TapError(action)
                : resultValueTask;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Action action)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure && predicate(result.Error))
                return result.TapError(action);

            return result;
        }


        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Action<TE> action)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

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
        public ValueTask<Result<T, TE>> TapErrorIf(bool condition, Func<ValueTask> func)
            => condition
                ? result.TapError(func)
                : ValueTask.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(bool condition, Func<TE, ValueTask> func)
            => condition
                ? result.TapError(func)
                : ValueTask.FromResult(result);

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<ValueTask> func)
            => result.IsFailure && predicate(result.Error)
                ? result.TapError(func)
                : ValueTask.FromResult(result);


        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public ValueTask<Result<T, TE>> TapErrorIf(Func<TE, bool> predicate, Func<TE, ValueTask> func)
            => result.IsFailure && predicate(result.Error)
                ? result.TapError(func)
                : ValueTask.FromResult(result);

    }
}

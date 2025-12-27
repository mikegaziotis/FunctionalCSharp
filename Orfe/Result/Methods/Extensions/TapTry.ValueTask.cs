using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapTry(Func<ValueTask> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            try
            {
                if (result.IsSuccess)
                    await func().ConfigureAwait(DefaultConfigureAwait);

                return result;
            }
            catch (Exception exc)
            {
                var error = errorHandler(exc);
                return new Result<T, TE>(true, error, default);
            }
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapTry(Func<T, ValueTask> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            try
            {
                if (result.IsSuccess)
                    await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

                return result;
            }
            catch (Exception exc)
            {
                var error = errorHandler(exc);
                return new Result<T, TE>(true, error, default);
            }
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapTry(Action action, Func<Exception, TE> errorHandler)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapTry(action, errorHandler);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapTry(Action<T> action, Func<Exception, TE> errorHandler)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapTry(action, errorHandler);
        }
    }


    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapTry(Func<ValueTask> func, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (result.IsSuccess)
                    await func().ConfigureAwait(DefaultConfigureAwait);

                return result;
            }
            catch (Exception exc)
            {
                var error = errorHandler(exc);
                return new Result<T, TE>(true, error, default);
            }
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async ValueTask<Result<T, TE>> TapTry(Func<T, ValueTask> func, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (result.IsSuccess)
                    await func(result.Value).ConfigureAwait(DefaultConfigureAwait);

                return result;
            }
            catch (Exception exc)
            {
                var error = errorHandler(exc);
                return new Result<T, TE>(true, error, default);
            }
        }
    }
}

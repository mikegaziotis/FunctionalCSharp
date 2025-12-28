using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapTry(Func<Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

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
        public async Task<Result<T, TE>> TapTry(Func<T, Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

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
        public async Task<Result<T, TE>> TapTry(Action action, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapTry(action, errorHandler);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public async Task<Result<T, TE>> TapTry(Action<T> action, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapTry(action, errorHandler);
        }
    }


    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapTry(Func<Task> func, Func<Exception, TE> errorHandler)
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
        public async Task<Result<T, TE>> TapTry(Func<T, Task> func, Func<Exception, TE> errorHandler)
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

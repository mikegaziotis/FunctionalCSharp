using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success and the condition is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(bool condition, Func<Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            try
            {
                if (condition && result.IsSuccess)
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
        ///     Executes the given action if the calling result is a success and the condition is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(bool condition, Func<T, Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            try
            {
                if (condition && result.IsSuccess)
                    await func(result.Value);

                return result;
            }
            catch (Exception exc)
            {
                var error = errorHandler(exc);
                return new Result<T, TE>(true, error, default);
            }
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and the predicate is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(Func<T, bool> predicate, Func<Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            try
            {
                if (result.IsSuccess && predicate(result.Value))
                    await func();

                return result;
            }
            catch (Exception exc)
            {
                var error = errorHandler(exc);
                return new Result<T, TE>(true, error, default);
            }
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and the predicate is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(Func<T, bool> predicate, Func<T, Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            try
            {
                if (result.IsSuccess && predicate(result.Value))
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
        ///     Executes the given action if the calling result is a success and the condition is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(bool condition, Action action, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapIfTry(condition, action, errorHandler);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and the condition is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(bool condition, Action<T> action, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapIfTry(condition, action, errorHandler);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and the predicate is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(Func<T, bool> predicate, Action action, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapIfTry(predicate, action, errorHandler);
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success and the predicate is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(Func<T, bool> predicate, Action<T> action, Func<Exception, TE> errorHandler)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.TapIfTry(predicate, action, errorHandler);
        }
    }


    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success and the condition is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(bool condition, Func<Task> func, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (condition && result.IsSuccess)
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
        ///     Executes the given action if the calling result is a success and the condition is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(bool condition, Func<T, Task> func, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (condition && result.IsSuccess)
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
        ///     Executes the given action if the calling result is a success and the predicate is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(Func<T, bool> predicate, Func<Task> func, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (result.IsSuccess && predicate(result.Value))
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
        ///     Executes the given action if the calling result is a success and the predicate is true. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public async Task<Result<T, TE>> TapIfTry(Func<T, bool> predicate, Func<T, Task> func, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (result.IsSuccess && predicate(result.Value))
                    await func(result.Value);

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

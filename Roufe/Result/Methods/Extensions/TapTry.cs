using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        ///     If there is an exception, returns a new failure Result.
        /// </summary>
        public Result<T, TE> TapTry(Action action, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (result.IsSuccess)
                    action();

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
        public Result<T, TE> TapTry(Action<T> action, Func<Exception, TE> errorHandler)
        {
            try
            {
                if (result.IsSuccess)
                    action(result.Value);

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

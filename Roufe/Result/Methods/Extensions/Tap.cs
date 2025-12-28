using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public Result<T, TE> Tap(Action action)
        {
            if (result.IsSuccess)
                action();

            return result;
        }

        /// <summary>
        ///     Executes the given action if the calling result is a success. Returns the calling result.
        /// </summary>
        public Result<T, TE> Tap(Action<T> action)
        {
            if (result.IsSuccess)
                action(result.Value);

            return result;
        }
    }
}

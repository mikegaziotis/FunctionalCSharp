using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapErrorIf(bool condition, Action action)
            => condition
                ? result.TapError(action)
                : result;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapErrorIf(bool condition, Action<TE> action)
            => condition
                ? result.TapError(action)
                : result;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapErrorIf(Func<TE, bool> predicate, Action action)
            => result.IsFailure && predicate(result.Error)
                ? result.TapError(action)
                : result;

        /// <summary>
        ///     Executes the given action if the calling result is a failure and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapErrorIf(Func<TE, bool> predicate, Action<TE> action)
            => result.IsFailure && predicate(result.Error)
                ? result.TapError(action)
                : result;
    }
}

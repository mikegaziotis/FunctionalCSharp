using System;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapIf(bool condition, Action action)
            => condition
                ? result.Tap(action)
                : result;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapIf(bool condition, Action<T> action)
            => condition
                ? result.Tap(action)
                : result;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapIf(Func<T, bool> predicate, Action action)
            => result.IsSuccess && predicate(result.Value)
                ? result.Tap(action)
                : result;

        /// <summary>
        ///     Executes the given action if the calling result is a success and condition is true. Returns the calling result.
        /// </summary>
        public Result<T, TE> TapIf(Func<T, bool> predicate, Action<T> action)
            => result.IsSuccess && predicate(result.Value)
                ? result.Tap(action)
                : result;
    }

}

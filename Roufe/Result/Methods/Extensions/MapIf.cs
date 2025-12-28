using System;

namespace Roufe;

/// <summary>
///     Creates a new result from the return value of a given function if the condition is true. If the calling Result is a failure, a new failure result is returned instead.
/// </summary>
public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Creates a new result from the return value of a given function if the condition is true. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public Result<T, TE> MapIf(bool condition, Func<T, T> func)
            => !condition
                ? result
                : result.Map(func);


        public Result<T, TE> MapIf<TContext>(bool condition, Func<T, TContext, T> func, TContext context)
            => !condition
                ? result :
                result.Map(func, context);

        /// <summary>
        ///     Creates a new result from the return value of a given function if the predicate is true. If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        public Result<T, TE> MapIf(Func<T, bool> predicate, Func<T, T> func)
            => !result.IsSuccess || !predicate(result.Value)
                ? result
                : result.Map(func);


        public Result<T, TE> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, T> func, TContext context)
            => !result.IsSuccess || !predicate(result.Value, context)
                ? result
                : result.Map(func, context);

    }
}

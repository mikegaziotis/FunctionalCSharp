using System;

namespace Orfe
{
    public static partial class ResultExtensions
    {

        extension<T, TE>(Result<T, TE> result)
        {
            /// <summary>
            ///     Selects result from the return value of a given function if the condition is true. If the calling Result is a failure, a new failure result is returned instead.
            /// </summary>
            public Result<T, TE> BindIf(bool condition, Func<T, Result<T, TE>> func)
            {
                return !condition
                    ? result
                    : result.Bind(func);
            }

            public Result<T, TE> BindIf(bool condition, Func<Result<T, TE>> func)
            {
                return !condition
                    ? result
                    : result.Bind(func);
            }

            /// <summary>
            ///     Selects result from the return value of a given function if the predicate is true. If the calling Result is a failure, a new failure result is returned instead.
            /// </summary>
            public Result<T, TE> BindIf(Func<T, bool> predicate, Func<T, Result<T, TE>> func)
            {
                return !result.IsSuccess || !predicate(result.Value)
                    ? result
                    : result.Bind(func);
            }

            /// <summary>
            ///     Selects result from the return value of a given function if the predicate is true. If the calling Result is a failure, a new failure result is returned instead.
            /// </summary>
            public Result<T, TE> BindIf(Func<T, bool> predicate, Func<Result<T, TE>> func)
            {
                return !result.IsSuccess || !predicate(result.Value)
                    ? result
                    : result.Bind(func);
            }

            /// <summary>
            ///     Selects result from the return value of a given function if the predicate is true. If the calling Result is a failure, a new failure result is returned instead.
            /// </summary>
            public Result<T, TE> BindIf(Func<bool> predicate, Func<Result<T, TE>> func)
            {
                return !result.IsSuccess || !predicate()
                    ? result
                    : result.Bind(func);
            }

            /// <summary>
            ///     Selects result from the return value of a given function if the predicate is true. If the calling Result is a failure, a new failure result is returned instead.
            /// </summary>
            public Result<T, TE> BindIf(Func<bool> predicate, Func<T, Result<T, TE>> func)
            {
                return !result.IsSuccess || !predicate()
                    ? result
                    : result.Bind(func);
            }
        }
    }
}

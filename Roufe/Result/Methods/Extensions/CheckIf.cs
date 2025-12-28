using System;

namespace Roufe
{
    public static partial class ResultExtensions
    {
        extension<T, TK, TE>(Result<T, TE> result)
        {
            /// <summary>
            ///     If the calling result is a success and the condition is true, the given function is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
            /// </summary>
            public Result<T, TE> CheckIf(bool condition, Func<T, Result<TK, TE>> func)
                => condition
                    ? result.Check(func)
                    : result;

            /// <summary>
            ///     If the calling result is a success and the predicate is true, the given function is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
            /// </summary>
            public Result<T, TE> CheckIf(Func<T, bool> predicate, Func<T, Result<TK, TE>> func)
                => result.IsSuccess && predicate(result.Value)
                    ? result.Check(func)
                    : result;
        }
    }
}

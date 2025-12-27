using System;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Selects result from the return value of a given function.
        ///     If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Result<TK, TE> Bind(Func<T, Result<TK, TE>> func)
        {
            return result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : func(result.Value);
        }

        /// <summary>
        ///     Selects result from the return value of a given parameterless function.
        ///     If the calling Result is a failure, a new failure result is returned instead.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Result<TK, TE> Bind(Func<Result<TK, TE>> func)
        {
            return result.IsFailure
                ? Result.Failure<TK, TE>(result.Error)
                : func();
        }

    }
}

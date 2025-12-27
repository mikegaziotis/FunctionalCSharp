using System;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {


        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public Result<T, TE> Ensure(Func<T, bool> predicate, TE error)
            => result.IsFailure
                ? result
                : !predicate(result.Value)
                        ? Result.Failure<T, TE>(error)
                        : result;

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public Result<T, TE> Ensure(Func<T, bool> predicate, Func<T, TE> errorPredicate)
            => result.IsFailure
                ? result
                : !predicate(result.Value)
                    ? Result.Failure<T, TE>(errorPredicate(result.Value))
                    : result;

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public Result<T, TE> Ensure(Func<T, bool> predicate, Func<TE> errorPredicate)
            => result.IsFailure
                ? result
                : !predicate(result.Value)
                    ? Result.Failure<T, TE>(errorPredicate())
                    : result;
    }

}

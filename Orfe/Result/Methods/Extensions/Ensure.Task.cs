using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, Task<bool>> predicate, TE error)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(error);

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, Task<bool>> predicate, Func<T, TE> errorPredicate)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(errorPredicate(result.Value));

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, Task<bool>> predicate, Func<T, Task<TE>> errorPredicate)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(await errorPredicate(result.Value).ConfigureAwait(DefaultConfigureAwait));

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, bool> predicate, TE error)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Ensure(predicate, error);
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, bool> predicate, Func<T, TE> errorPredicate)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Ensure(predicate, errorPredicate);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, Task<bool>> predicate, TE error)
        {
            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(error);

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, Task<bool>> predicate, Func<T, TE> errorPredicate)
        {
            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(errorPredicate(result.Value));

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async Task<Result<T, TE>> Ensure(Func<T, Task<bool>> predicate, Func<T, Task<TE>> errorPredicate)
        {
            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(await errorPredicate(result.Value).ConfigureAwait(DefaultConfigureAwait));

            return result;
        }
    }
}

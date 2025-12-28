using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{

    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, ValueTask<bool>> predicate, TE error)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(error);

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, ValueTask<bool>> predicate, Func<T, TE> errorPredicate)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(errorPredicate(result.Value));

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, ValueTask<bool>> predicate, Func<T, ValueTask<TE>> errorPredicate)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(await errorPredicate(result.Value).ConfigureAwait(DefaultConfigureAwait));

            return result;
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, bool> predicate, TE error)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Ensure(predicate, error);
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, bool> predicate, Func<T, TE> errorPredicate)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Ensure(predicate, errorPredicate);
        }

        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, bool> predicate, Func<TE> errorPredicate)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Ensure(predicate, errorPredicate);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Returns a new failure result if the predicate is false. Otherwise, returns the starting result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Ensure(Func<T, ValueTask<bool>> predicate, TE error)
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
        public async ValueTask<Result<T, TE>> Ensure(Func<T, ValueTask<bool>> predicate, Func<T, TE> errorPredicate)
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
        public async ValueTask<Result<T, TE>> Ensure(Func<T, ValueTask<bool>> predicate, Func<T, ValueTask<TE>> errorPredicate)
        {
            if (result.IsFailure)
                return result;

            if (!await predicate(result.Value).ConfigureAwait(DefaultConfigureAwait))
                return Result.Failure<T, TE>(await errorPredicate(result.Value).ConfigureAwait(DefaultConfigureAwait));

            return result;
        }
    }
}

using System.Threading.Tasks;
using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        /// Async variant: Maps both success and error values on a Task Result.
        /// </summary>
        public async Task<Result<TK, TR>> BiMap<TK, TR>(
            Func<T, TK> onSuccess,
            Func<TE, TR> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BiMap(onSuccess, onFailure);
        }

        /// <summary>
        /// Async variant: Maps both success and error values with async functions.
        /// </summary>
        public async Task<Result<TK, TR>> BiMapAsync<TK, TR>(
            Func<T, Task<TK>> onSuccess,
            Func<TE, Task<TR>> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsSuccess
                ? Result.Success<TK, TR>(await onSuccess(result.Value).ConfigureAwait(DefaultConfigureAwait))
                : Result.Failure<TK, TR>(await onFailure(result.Error).ConfigureAwait(DefaultConfigureAwait));
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        /// Maps both success and error values with async functions.
        /// </summary>
        public async Task<Result<TK, TR>> BiMapAsync<TK, TR>(
            Func<T, Task<TK>> onSuccess,
            Func<TE, Task<TR>> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            return result.IsSuccess
                ? Result.Success<TK, TR>(await onSuccess(result.Value).ConfigureAwait(DefaultConfigureAwait))
                : Result.Failure<TK, TR>(await onFailure(result.Error).ConfigureAwait(DefaultConfigureAwait));
        }
    }
}


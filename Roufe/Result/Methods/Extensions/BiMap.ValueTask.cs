using System.Threading.Tasks;
using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        /// <summary>
        /// ValueTask variant: Maps both success and error values on a ValueTask Result.
        /// </summary>
        public async ValueTask<Result<TK, TR>> BiMap<TK, TR>(
            Func<T, TK> onSuccess,
            Func<TE, TR> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BiMap(onSuccess, onFailure);
        }

        /// <summary>
        /// ValueTask variant: Maps both success and error values with async functions.
        /// </summary>
        public async ValueTask<Result<TK, TR>> BiMapAsync<TK, TR>(
            Func<T, ValueTask<TK>> onSuccess,
            Func<TE, ValueTask<TR>> onFailure)
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
        /// Maps both success and error values with async ValueTask functions.
        /// </summary>
        public async ValueTask<Result<TK, TEK>> BiMapAsync<TK, TEK>(
            Func<T, ValueTask<TK>> onSuccess,
            Func<TE, ValueTask<TEK>> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            return result.IsSuccess
                ? Result.Success<TK, TEK>(await onSuccess(result.Value).ConfigureAwait(DefaultConfigureAwait))
                : Result.Failure<TK, TEK>(await onFailure(result.Error).ConfigureAwait(DefaultConfigureAwait));
        }
    }
}


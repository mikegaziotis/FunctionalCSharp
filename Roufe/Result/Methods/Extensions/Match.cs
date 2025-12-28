using System;

namespace Roufe
{
    public static partial class ResultExtensions
    {
        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        /// T
        public static TK Match<T, TK, TE>(this Result<T, TE> result, Func<T, TK> onSuccess, Func<TE, TK> onFailure)
            => result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);


        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        /// T
        public static void Match<T, TE>(this Result<T, TE> result, Action<T> onSuccess, Action<TE> onFailure)
        {
            if (result.IsSuccess)
                onSuccess(result.Value);
            else
                onFailure(result.Error);
        }
    }
}

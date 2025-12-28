using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        /// Maps both success and error values simultaneously.
        /// If the Result is success, applies the success function to the value.
        /// If the Result is failure, applies the error function to the error.
        /// </summary>
        /// <example>
        /// Result&lt;int, string&gt; result = Result.Success&lt;int, string&gt;(42);
        /// Result&lt;string, int&gt; mapped = result.BiMap(
        ///     value => value.ToString(),
        ///     error => error.Length);
        /// // mapped.Value = "42"
        /// </example>
        public Result<TK, TR> BiMap<TK, TR>(
            Func<T, TK> onSuccess,
            Func<TE, TR> onFailure)
        {
            ArgumentNullException.ThrowIfNull(onSuccess);
            ArgumentNullException.ThrowIfNull(onFailure);

            return result.IsSuccess
                ? Result.Success<TK, TR>(onSuccess(result.Value))
                : Result.Failure<TK, TR>(onFailure(result.Error));
        }
    }
}


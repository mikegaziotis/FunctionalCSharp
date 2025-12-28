using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T,TE>(in Result<T,TE> result)
    {
        /// <summary>
        ///     Gets the value of the result on success, otherwise returns the value returned by the function.
        /// </summary>
        public T GetValueOrDefault(Func<T> defaultValue)
            => result.IsFailure
                ? defaultValue()
                : result.Value;

        /// <summary>
        ///     Gets the value of the result on success, otherwise returns the value returned by the selector when called with the default value function.
        /// </summary>
        public TK? GetValueOrDefault<TK>(Func<T, TK> selector, TK? defaultValue = default)
        {
            return result.IsFailure ? defaultValue : selector(result.Value);
        }

        /// <summary>
        ///     Gets the value of the result on success, otherwise returns the value returned by the selector when called with the default value function.
        /// </summary>
        public TK GetValueOrDefault<TK>(Func<T, TK> selector, Func<TK> defaultValue)
            => result.IsFailure ? defaultValue() : selector(result.Value);

    }
}

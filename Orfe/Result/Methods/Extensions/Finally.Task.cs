using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TK, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Passes the result to the given function (regardless of success/failure state) to yield a final output value.
        /// </summary>
        public async Task<TK> Finally(Func<Result<T, TE>, Task<TK>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await func(result).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     Passes the result to the given function (regardless of success/failure state) to yield a final output value.
        /// </summary>
        public async Task<TK> Finally(Func<Result<T, TE>, TK> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Finally(func);
        }
    }

    /// <summary>
    ///     Passes the result to the given function (regardless of success/failure state) to yield a final output value.
    /// </summary>
    public static Task<TK> Finally<T, TK, TE>(this Result<T, TE> result, Func<Result<T, TE>, Task<TK>> func)
        => func(result);
}

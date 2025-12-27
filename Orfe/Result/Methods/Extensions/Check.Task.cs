using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     If the calling result is a success, the given function is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
        /// </summary>
        public async Task<Result<T, TE>> Check(Func<T, Result<TK, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Check(func);
        }

        /// <summary>
        ///     If the calling result is a success, the given function is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
        /// </summary>
        public async Task<Result<T, TE>> Check(Func<T, Task<Result<TK, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Bind(func).Map(_ => result.Value).ConfigureAwait(DefaultConfigureAwait);
        }
    }

    /// <summary>
    ///     If the calling result is a success, the given function is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
    /// </summary>
    public static async Task<Result<T, TE>> Check<T, TK, TE>(this Result<T, TE> result, Func<T, Task<Result<TK, TE>>> func)
        => await result.Bind(func).Map(_ => result.Value).ConfigureAwait(DefaultConfigureAwait);
}

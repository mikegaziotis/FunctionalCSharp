using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TK, TE, TR>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     This method should be used in linq queries. We recommend using Bind method.
        /// </summary>
        public async Task<Result<TR, TE>> SelectMany(Func<T, Task<Result<TK, TE>>> func, Func<T, TK, TR> project)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.SelectMany(func, project).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     This method should be used in linq queries. We recommend using Bind method.
        /// </summary>
        public async Task<Result<TR, TE>> SelectMany(Func<T, Result<TK, TE>> func, Func<T, TK, TR> project)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.SelectMany(func, project);
        }
    }

    /// <summary>
    ///     This method should be used in linq queries. We recommend using Bind method.
    /// </summary>
    public static Task<Result<TR, TE>> SelectMany<T, TK, TE, TR>(this Result<T, TE> result, Func<T, Task<Result<TK, TE>>> func, Func<T, TK, TR> project)
        => result
            .Bind(func)
            .Map(x => project(result.Value, x));
}

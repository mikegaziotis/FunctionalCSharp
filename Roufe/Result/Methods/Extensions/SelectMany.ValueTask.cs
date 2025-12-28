using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE, TR>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     This method should be used in linq queries. We recommend using Bind method.
        /// </summary>
        public async ValueTask<Result<TR, TE>> SelectMany(Func<T, ValueTask<Result<TK, TE>>> func, Func<T, TK, TR> project)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.SelectMany(func, project).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     This method should be used in linq queries. We recommend using Bind method.
        /// </summary>
        public async ValueTask<Result<TR, TE>> SelectMany(Func<T, Result<TK, TE>> func, Func<T, TK, TR> project)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.SelectMany(func, project);
        }
    }

    /// <summary>
    ///     This method should be used in linq queries. We recommend using Bind method.
    /// </summary>
    public static async ValueTask<Result<TR, TE>> SelectMany<T, TK, TE, TR>(this Result<T, TE> result,
        Func<T, ValueTask<Result<TK, TE>>> func,
        Func<T, TK, TR> project)
    {
        var bound = await result.Bind(func).ConfigureAwait(false);
        return bound.Map(x => project(result.Value, x));
    }

}

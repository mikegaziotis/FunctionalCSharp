using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     If the calling result is a success, the given valueTask action is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
        /// </summary>
        public async ValueTask<Result<T, TE>> Check(Func<T, ValueTask<Result<TK, TE>>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Bind(valueTask).Map(_ => result.Value).ConfigureAwait(DefaultConfigureAwait);
        }

        /// <summary>
        ///     If the calling result is a success, the given valueTask action is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
        /// </summary>
        public async ValueTask<Result<T, TE>> Check(Func<T, Result<TK, TE>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Check(valueTask);
        }
    }

    /// <summary>
    ///     If the calling result is a success, the given valueTask action is executed and its Result is checked. If this Result is a failure, it is returned. Otherwise, the calling result is returned.
    /// </summary>
    public static async ValueTask<Result<T, TE>> Check<T, TK, TE>(this Result<T, TE> result, Func<T, ValueTask<Result<TK, TE>>> valueTask)
        => await result.Bind(valueTask).Map(_ => result.Value).ConfigureAwait(DefaultConfigureAwait);

}

using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {

        public async Task<Result<T, TE2>> Compensate<TE2>(Func<TE, Task<Result<T, TE2>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Compensate(func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE2>> Compensate<TE2>(Func<TE, Result<T, TE2>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Compensate(func);
        }
    }

    public static Task<Result<T, TE2>> Compensate<T, TE, TE2>(this Result<T, TE> result, Func<TE, Task<Result<T, TE2>>> func)
        => result.IsSuccess
            ? Result.Success<T, TE2>(result.Value).AsCompletedTask()
            : func(result.Error);
}

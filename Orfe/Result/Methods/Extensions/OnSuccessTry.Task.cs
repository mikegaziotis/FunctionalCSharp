using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TE>(Task<Result<T,TE>> task)
    {
        public async Task<Result<Unit,TE>> OnSuccessTry(Func<T, Task> func, Func<Exception, TE> errorHandler)
        {
            var result = await task.ConfigureAwait(DefaultConfigureAwait);
            return await result.OnSuccessTry(func, errorHandler).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<Unit,TE>> OnSuccessTry(Action<T> action, Func<Exception, TE> errorHandler)
        {
            var result = await task.ConfigureAwait(DefaultConfigureAwait);
            return result.OnSuccessTry(action, errorHandler);
        }

    }

    public static async Task<Result<Unit,TE>> OnSuccessTry<T,TE>(this Result<T,TE> result, Func<T, Task> func, Func<Exception, TE> errorHandler)
        => result.IsFailure
            ? Result.Failure(result.Error)
            : await Result.Try(() => func.Invoke(result.Value), errorHandler).ConfigureAwait(DefaultConfigureAwait);
}

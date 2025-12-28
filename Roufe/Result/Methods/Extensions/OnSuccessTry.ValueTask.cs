using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{

    extension<T, TE>(ValueTask<Result<T,TE>> valueTask)
    {
        public async ValueTask<Result<Unit,TE>> OnSuccessTry(Func<T, ValueTask> func, Func<Exception, TE> errorHandler)
        {
            var result = await valueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.OnSuccessTry(func, errorHandler).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<Unit,TE>> OnSuccessTry(Action<T> action, Func<Exception, TE> errorHandler)
        {
            var result = await valueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.OnSuccessTry(action, errorHandler);
        }

    }

    public static async ValueTask<Result<Unit,TE>> OnSuccessTry<T,TE>(this Result<T,TE> result, Func<T, ValueTask> func, Func<Exception, TE> errorHandler)
        => result.IsFailure
            ? Result.Failure(result.Error)
            : await Result.Try(() => func.Invoke(result.Value), errorHandler).ConfigureAwait(DefaultConfigureAwait);
}

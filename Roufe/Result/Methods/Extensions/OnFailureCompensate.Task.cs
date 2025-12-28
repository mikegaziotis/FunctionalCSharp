using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        public async Task<Result<T, TE>> OnFailureCompensate(Func<Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsFailure
                ? await func().ConfigureAwait(DefaultConfigureAwait)
                : result;
        }

        public async Task<Result<T, TE>> OnFailureCompensate(Func<TE, Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsFailure
                ? await func(result.Error).ConfigureAwait(DefaultConfigureAwait)
                : result;
        }

        public async Task<Result<T, TE>> OnFailureCompensate(Func<Result<T, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.OnFailureCompensate(func);
        }

        public async Task<Result<T, TE>> OnFailureCompensate(Func<TE, Result<T, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.OnFailureCompensate(func);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        public async Task<Result<T, TE>> OnFailureCompensate(Func<Task<Result<T, TE>>> func)
            => result.IsFailure
                ? await func().ConfigureAwait(DefaultConfigureAwait)
                : result;

        public async Task<Result<T, TE>> OnFailureCompensate(Func<TE, Task<Result<T, TE>>> func)
            => result.IsFailure
                ? await func(result.Error).ConfigureAwait(DefaultConfigureAwait)
                : result;
    }
}

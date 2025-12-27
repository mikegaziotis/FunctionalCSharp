using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        public async ValueTask<Result<T, TE>> OnFailureCompensate(Func<ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsFailure
                ? await func().ConfigureAwait(DefaultConfigureAwait)
                : result;
        }

        public async ValueTask<Result<T, TE>> OnFailureCompensate(Func<TE, ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsFailure
                ? await func(result.Error).ConfigureAwait(DefaultConfigureAwait)
                : result;
        }

        public async ValueTask<Result<T, TE>> OnFailureCompensate(Func<Result<T, TE>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.OnFailureCompensate(func);
        }

        public async ValueTask<Result<T, TE>> OnFailureCompensate(Func<TE, Result<T, TE>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.OnFailureCompensate(func);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        public async ValueTask<Result<T, TE>> OnFailureCompensate(Func<ValueTask<Result<T, TE>>> func)
            => result.IsFailure
                ? await func().ConfigureAwait(DefaultConfigureAwait)
                : result;

        public async ValueTask<Result<T, TE>> OnFailureCompensate(Func<TE, ValueTask<Result<T, TE>>> func)
            => result.IsFailure
                ? await func(result.Error).ConfigureAwait(DefaultConfigureAwait)
                : result;
    }
}

using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{

    extension<T, TK, TE>(Task<Result<T, TE>> resultTask)
    {
        public async Task<Result<T, TE>> CheckIf(Func<T, bool> predicate, Func<T, Task<Result<TK, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && predicate(result.Value))
                return await result.Check(func).ConfigureAwait(DefaultConfigureAwait);

            return result;
        }

        public async Task<Result<T, TE>> CheckIf(Func<T, bool> predicate, Func<T, Result<TK, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsSuccess && predicate(result.Value))
                return result.Check(func);

            return result;
        }

        public Task<Result<T, TE>> CheckIf(bool condition, Func<T, Result<TK, TE>> func)
            => condition ? resultTask.Check(func) : resultTask;
    }

    extension<T, TK, TE>(Result<T, TE> result)
    {
        public Task<Result<T, TE>> CheckIf(Func<T, bool> predicate, Func<T, Task<Result<TK, TE>>> func)
            => result.IsSuccess && predicate(result.Value)
                ? result.Check(func)
                : Task.FromResult(result);

        public Task<Result<T, TE>> CheckIf(bool condition, Func<T, Task<Result<TK, TE>>> func)
            => condition ? result.Check(func) : Task.FromResult(result);
    }
}

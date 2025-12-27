using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {

        public async Task<Result<T, TE>> BindIf(bool condition, Func<Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(condition, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> BindIf(bool condition, Func<T, Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(condition, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> BindIf(bool condition, Func<T, Result<T, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(condition, func);
        }

        public async Task<Result<T, TE>> BindIf(bool condition, Func<Result<T, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(condition, func);
        }

        public async Task<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<T, Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> BindIf(Func<bool> predicate, Func<Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> BindIf(Func<bool> predicate, Func<T, Task<Result<T, TE>>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> BindIf(Func<bool> predicate, Func<Result<T, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(predicate, func);
        }

        public async Task<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<T, Result<T, TE>> func)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(predicate, func);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        public async Task<Result<T, TE>> BindIf(bool condition, Func<T, Task<Result<T, TE>>> func)
            => !condition
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async Task<Result<T, TE>> BindIf(bool condition, Func<Task<Result<T, TE>>> func)
            => !condition
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async Task<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<T, Task<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate(result.Value)
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async Task<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<Task<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate(result.Value)
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async Task<Result<T, TE>> BindIf(Func<bool> predicate, Func<Task<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate()
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public Task<Result<T, TE>> BindIf(Func<bool> predicate, Func<T, Task<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate()
                ? result.AsCompletedTask()
                : result.Bind(func);
    }
}

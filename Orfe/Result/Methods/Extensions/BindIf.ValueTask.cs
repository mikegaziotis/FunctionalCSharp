using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {

        public async ValueTask<Result<T, TE>> BindIf(bool condition, Func<ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(condition, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> BindIf(bool condition, Func<T, ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(condition, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> BindIf(bool condition, Func<T, Result<T, TE>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(condition, func);
        }

        public async ValueTask<Result<T, TE>> BindIf(bool condition, Func<Result<T, TE>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(condition, func);
        }

        public async ValueTask<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<T, ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> BindIf(Func<bool> predicate, Func<ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> BindIf(Func<bool> predicate, Func<T, ValueTask<Result<T, TE>>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.BindIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> BindIf(Func<bool> predicate, Func<Result<T, TE>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(predicate, func);
        }

        public async ValueTask<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<T, Result<T, TE>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.BindIf(predicate, func);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        public async ValueTask<Result<T, TE>> BindIf(bool condition, Func<T, ValueTask<Result<T, TE>>> func)
            => !condition
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async ValueTask<Result<T, TE>> BindIf(bool condition, Func<ValueTask<Result<T, TE>>> func)
            => !condition
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async ValueTask<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<T, ValueTask<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate(result.Value)
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async ValueTask<Result<T, TE>> BindIf(Func<T, bool> predicate, Func<ValueTask<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate(result.Value)
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async ValueTask<Result<T, TE>> BindIf(Func<bool> predicate, Func<ValueTask<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate()
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);

        public async ValueTask<Result<T, TE>> BindIf(Func<bool> predicate, Func<T, ValueTask<Result<T, TE>>> func)
            => !result.IsSuccess || !predicate()
                ? result
                : await result.Bind(func).ConfigureAwait(DefaultConfigureAwait);
    }
}

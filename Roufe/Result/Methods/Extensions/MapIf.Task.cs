using System;
using System.Threading.Tasks;

namespace Roufe
{
    public static partial class ResultExtensions
    {
        extension<T, TE>(Task<Result<T, TE>> resultTask)
        {
            public async Task<Result<T, TE>> MapIf(bool condition, Func<T, Task<T>> func)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.MapIf(condition, func).ConfigureAwait(DefaultConfigureAwait);
            }

            public async Task<Result<T, TE>> MapIf<TContext>(bool condition, Func<T, TContext, Task<T>> func, TContext context)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.MapIf(condition, func, context).ConfigureAwait(DefaultConfigureAwait);
            }

            public async Task<Result<T, TE>> MapIf(Func<T, bool> predicate, Func<T, Task<T>> func)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.MapIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
            }

            public async Task<Result<T, TE>> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, Task<T>> func, TContext context)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return await result.MapIf(predicate, func, context).ConfigureAwait(DefaultConfigureAwait);
            }

            public async Task<Result<T, TE>> MapIf(bool condition, Func<T, T> func)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.MapIf(condition, func);
            }

            public async Task<Result<T, TE>> MapIf<TContext>(bool condition, Func<T, TContext, T> func, TContext context)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.MapIf(condition, func, context);
            }

            public async Task<Result<T, TE>> MapIf(Func<T, bool> predicate, Func<T, T> func)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.MapIf(predicate, func);
            }

            public async Task<Result<T, TE>> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, T> func, TContext context)
            {
                var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
                return result.MapIf(predicate, func, context);
            }
        }

        extension<T, TE>(Result<T, TE> result)
        {
            public Task<Result<T, TE>> MapIf(bool condition, Func<T, Task<T>> func)
                => !condition
                    ? result.AsCompletedTask()
                    : result.Map(func);

            public Task<Result<T, TE>> MapIf<TContext>(bool condition, Func<T, TContext, Task<T>> func, TContext context)
                => !condition
                    ? result.AsCompletedTask()
                    : result.Map(func, context);

            public Task<Result<T, TE>> MapIf(Func<T, bool> predicate, Func<T, Task<T>> func)
                => !result.IsSuccess || !predicate(result.Value)
                    ? result.AsCompletedTask()
                    : result.Map(func);

            public Task<Result<T, TE>> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, Task<T>> func, TContext context)
                => !result.IsSuccess || !predicate(result.Value, context)
                    ? result.AsCompletedTask()
                    : result.Map(func, context);

        }
    }
}

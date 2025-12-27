using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        public async ValueTask<Result<T, TE>> MapIf(bool condition, Func<T, ValueTask<T>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapIf(condition, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> MapIf<TContext>(bool condition, Func<T, TContext, ValueTask<T>> func, TContext context)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapIf(condition, func, context).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> MapIf(Func<T, bool> predicate, Func<T, ValueTask<T>> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapIf(predicate, func).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, ValueTask<T>> func, TContext context)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.MapIf(predicate, func, context).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> MapIf(bool condition, Func<T, T> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.MapIf(condition, func);
        }

        public async ValueTask<Result<T, TE>> MapIf<TContext>(bool condition, Func<T, TContext, T> func, TContext context)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.MapIf(condition, func, context);
        }

        public async ValueTask<Result<T, TE>> MapIf(Func<T, bool> predicate, Func<T, T> func)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.MapIf(predicate, func);
        }

        public async ValueTask<Result<T, TE>> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, T> func, TContext context)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            return result.MapIf(predicate, func, context);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        public ValueTask<Result<T, TE>> MapIf(bool condition, Func<T, ValueTask<T>> func)
            => !condition
                ? result.AsCompletedValueTask()
                : result.Map(func);

        public ValueTask<Result<T, TE>> MapIf<TContext>(bool condition, Func<T, TContext, ValueTask<T>> func, TContext context)
            => !condition
                ? result.AsCompletedValueTask()
                : result.Map(func, context);

        public ValueTask<Result<T, TE>> MapIf(Func<T, bool> predicate, Func<T, ValueTask<T>> func)
            => !result.IsSuccess || !predicate(result.Value)
                ? result.AsCompletedValueTask()
                : result.Map(func);

        public ValueTask<Result<T, TE>> MapIf<TContext>(Func<T, TContext, bool> predicate, Func<T, TContext, ValueTask<T>> func, TContext context)
            => !result.IsSuccess || !predicate(result.Value, context)
                ? result.AsCompletedValueTask()
                : result.Map(func, context);

    }
}

using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{

    extension<T, TK, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        public async ValueTask<Result<T, TE>> CheckIf(Func<T, bool> predicate, Func<T, Result<TK, TE>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsSuccess && predicate(result.Value)
                ? result.Check(valueTask)
                : result;
        }

        public ValueTask<Result<T, TE>> CheckIf(bool condition, Func<T, Result<TK, TE>> valueTask)
            => condition
                ? resultTask.Check(valueTask)
                : resultTask;

        public async ValueTask<Result<T, TE>> CheckIf(Func<T, bool> predicate, Func<T, ValueTask<Result<TK, TE>>> valueTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.IsSuccess && predicate(result.Value)
                ? await result.Check(valueTask).ConfigureAwait(DefaultConfigureAwait)
                : result;
        }
    }

    extension<T, TK, TE>(Result<T, TE> result)
    {
        public ValueTask<Result<T, TE>> CheckIf(bool condition, Func<T, ValueTask<Result<TK, TE>>> valueTask)
            =>  condition
                ? result.Check(valueTask)
                : result.AsCompletedValueTask();

        public ValueTask<Result<T, TE>> CheckIf(Func<bool> predicate, Func<T, ValueTask<Result<TK, TE>>> valueTask)
            => result.IsSuccess && predicate()
                ? result.Check(valueTask)
                : result.AsCompletedValueTask();

        public ValueTask<Result<T, TE>> CheckIf(Func<T, bool> predicate, Func<T, ValueTask<Result<TK, TE>>> valueTask)
            => result.IsSuccess && predicate(result.Value)
                ? result.Check(valueTask)
                : result.AsCompletedValueTask();
    }
}

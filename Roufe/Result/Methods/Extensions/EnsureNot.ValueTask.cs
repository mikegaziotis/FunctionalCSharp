using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;


public static partial class ResultExtensions
{
    /// <summary>
    ///     Returns a new failure result if the predicate is true. Otherwise, returns the starting result.
    /// </summary>
    public static async ValueTask<Result<T, TE>> EnsureNot<T, TE>(this Result<T, TE> result, Func<T, ValueTask<bool>> test, TE error)
    {
        return await result.Ensure(NegateTest, error).ConfigureAwait(DefaultConfigureAwait);

        async ValueTask<bool> NegateTest(T value)
            => !await test(value).ConfigureAwait(DefaultConfigureAwait);
    }

    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        public async ValueTask<Result<T, TE>> EnsureNot(Func<T, ValueTask<bool>> test, TE error)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            return await result.Ensure(NegateTest, error).ConfigureAwait(DefaultConfigureAwait);

            async ValueTask<bool> NegateTest(T value)
                => !await test(value).ConfigureAwait(DefaultConfigureAwait);
        }

        public async ValueTask<Result<T, TE>> EnsureNot(Func<T, bool> test, TE error)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);

            return result.Ensure(v => !test(v), error);
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Orfe;


public static partial class ResultExtensions
{
    /// <summary>
    ///     Returns a new failure result if the predicate is true. Otherwise, returns the starting result.
    /// </summary>
    public static async Task<Result<T, TE>> EnsureNot<T, TE>(this Result<T, TE> result, Func<T, Task<bool>> test, TE error)
    {
        return await result.Ensure(NegateTest, error).ConfigureAwait(DefaultConfigureAwait);

        async Task<bool> NegateTest(T value)
            => !await test(value).ConfigureAwait(DefaultConfigureAwait);
    }

    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        public async Task<Result<T, TE>> EnsureNot(Func<T, Task<bool>> test, TE error)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return await result.Ensure(NegateTest, error).ConfigureAwait(DefaultConfigureAwait);

            async Task<bool> NegateTest(T value)
                => !await test(value).ConfigureAwait(DefaultConfigureAwait);
        }

        public async Task<Result<T, TE>> EnsureNot(Func<T, bool> test, TE error)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.Ensure(v => !test(v), error);
        }
    }
}

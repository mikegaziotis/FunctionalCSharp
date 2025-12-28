using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<Result<T, TE>, TE>> resultTask)
    {
        /// <summary>
        /// ValueTask variant: Flattens a ValueTask of nested Result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Flatten()
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Flatten();
        }
    }

    extension<T, TE>(Result<ValueTask<Result<T, TE>>, TE> result)
    {
        /// <summary>
        /// Flattens a Result containing a ValueTask of Result.
        /// If the outer Result is failure, returns that failure wrapped in a ValueTask.
        /// If the outer Result is success, returns the inner ValueTask Result.
        /// </summary>
        public ValueTask<Result<T, TE>> Flatten()
        {
            return result.IsFailure
                ? ValueTask.FromResult(Result.Failure<T, TE>(result.Error))
                : result.Value;
        }
    }

    extension<T, TE>(ValueTask<Result<ValueTask<Result<T, TE>>, TE>> resultTask)
    {
        /// <summary>
        /// ValueTask variant: Flattens a ValueTask of Result containing a ValueTask of Result.
        /// </summary>
        public async ValueTask<Result<T, TE>> Flatten()
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Flatten().ConfigureAwait(DefaultConfigureAwait);
        }
    }
}


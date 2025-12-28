using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<Result<T, TE>, TE>> resultTask)
    {
        /// <summary>
        /// Async variant: Flattens a Task of nested Result.
        /// </summary>
        public async Task<Result<T, TE>> Flatten()
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Flatten();
        }
    }

    extension<T, TE>(Result<Task<Result<T, TE>>, TE> result)
    {
        /// <summary>
        /// Flattens a Result containing a Task of Result.
        /// If the outer Result is failure, returns that failure wrapped in a Task.
        /// If the outer Result is success, returns the inner Task Result.
        /// </summary>
        public Task<Result<T, TE>> Flatten()
        {
            return result.IsFailure
                ? Task.FromResult(Result.Failure<T, TE>(result.Error))
                : result.Value;
        }
    }

    extension<T, TE>(Task<Result<Task<Result<T, TE>>, TE>> resultTask)
    {
        /// <summary>
        /// Async variant: Flattens a Task of Result containing a Task of Result.
        /// </summary>
        public async Task<Result<T, TE>> Flatten()
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            return await result.Flatten().ConfigureAwait(DefaultConfigureAwait);
        }
    }
}


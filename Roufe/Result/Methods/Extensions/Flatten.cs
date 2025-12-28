namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<Result<T, TE>, TE> result)
    {
        /// <summary>
        /// Flattens a nested Result into a single-level Result.
        /// If the outer Result is failure, returns that failure.
        /// If the outer Result is success, returns the inner Result.
        /// </summary>
        /// <example>
        /// Result&lt;Result&lt;int, string&gt;, string&gt; nested = Result.Success(Result.Success&lt;int, string&gt;(42));
        /// Result&lt;int, string&gt; flattened = nested.Flatten();
        /// // flattened.Value = 42
        /// </example>
        public Result<T, TE> Flatten()
        {
            return result.IsFailure
                ? Result.Failure<T, TE>(result.Error)
                : result.Value;
        }
    }
}


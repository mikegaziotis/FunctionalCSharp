using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Task<Result<Option<T>, TE>> resultTask)
    {
        /// <summary>
        /// Change from one result type to another, while staying optional (the return type still contains <see cref="Option"/>)
        /// Even though it's a Result&lt;Option&gt;, the mapping function only has to
        /// operate on the innermost value, and is only called if it's present.
        /// </summary>
        public async Task<Result<Option<TK>, TE>> BindOptional(Func<T, Result<TK, TE>> bind)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return result.BindOptional(bind);
        }

        /// <summary>
        /// Change from one result type to another, while staying optional (the return type still contains <see cref="Option"/>)
        /// Even though it's a Result&lt;Option&gt;, the mapping function only has to
        /// operate on the innermost value, and is only called if it's present.
        /// </summary>
        public async Task<Result<Option<TK>, TE>> BindOptional(Func<T, Task<Result<TK, TE>>> bind)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            if (result.IsFailure)
                return Result.Failure<Option<TK>, TE>(result.Error);

            return await result.Value.Match(
                some:  v => bind(v).Map(Option.From),
                none: () => Task.FromResult(Result.Success<Option<TK>, TE>(Option.None)))
                    .ConfigureAwait(DefaultConfigureAwait);
        }
    }
}

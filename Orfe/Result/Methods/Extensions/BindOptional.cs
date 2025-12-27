using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class ResultExtensions
{
    /// <summary>
    /// Change from one result type to another, while staying optional (the return type still contains <see cref="Option"/>)
    /// Even though it's a Result&lt;Option&gt;, the mapping function only has to
    /// operate on the innermost value, and is only called if it's present.
    /// </summary>
    public static Result<Option<TK>, TE> BindOptional<T, TK, TE>(
        this Result<Option<T>, TE> result,
        Func<T, Result<TK, TE>> bind)
    {
        if (result.IsFailure)
            return Result.Failure<Option<TK>, TE>(result.Error);

        return result.Value.Match(
            some: v => bind(v).Map(Option.From),
            none: () => Result.Success<Option<TK>, TE>(Option.None));
    }


}

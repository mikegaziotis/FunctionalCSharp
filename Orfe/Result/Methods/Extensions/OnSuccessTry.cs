using System;

namespace Orfe;

public static partial class ResultExtensions
{

    public static Result<Unit,TE> OnSuccessTry<T,TE>(this Result<T,TE> result, Action<T> action, Func<Exception, TE> errorHandler)
        => result.IsFailure
            ? result.Error
            : Result.Try(() => action(result.Value), errorHandler);
}

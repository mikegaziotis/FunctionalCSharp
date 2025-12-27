using System;

namespace Orfe;

public partial struct Result<T, TE>
{
    /// <summary>
    ///     Throws if the result is a success. Else returns a new failure result of the given type.
    /// </summary>
    public Result<TK, TE> ConvertFailure<TK>()
        =>  IsSuccess
            ? throw new InvalidOperationException(Result.Messages.ConvertFailureExceptionOnSuccess)
            : Result.Failure<TK, TE>(Error);

}

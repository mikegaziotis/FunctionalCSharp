using System;

namespace Roufe;

public partial struct Result
{


    /// <summary>
    ///     Creates a result whose success/failure reflects the supplied condition. Opposite of FailureIf().
    /// </summary>
    public static Result<T, TE> SuccessIf<T, TE>(bool isSuccess, in T? value, in TE? error)
        => isSuccess
            ? Success<T, TE>(value)
            : Failure<T, TE>(error);


    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of FailureIf().
    /// </summary>
    public static Result<T, TE> SuccessIf<T, TE>(Func<bool> predicate, in T value, in TE error)
        => SuccessIf(predicate(), value, error);

    /// <summary>
    ///     Creates a result whose success/failure reflects the supplied condition. Opposite of FailureIf().
    /// </summary>
    public static Result<Unit,TE> SuccessIf<TE>(bool isSuccess, in TE? error)
        => isSuccess
            ? Success<Unit,TE>(Unit.Value)
            : Failure<Unit,TE>(error);


    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of FailureIf().
    /// </summary>
    public static Result<Unit,TE> SuccessIf<TE>(Func<bool> predicate, in TE? error)
        => SuccessIf(predicate(), error);

}

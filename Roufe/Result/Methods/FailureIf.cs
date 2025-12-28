using System;
using System.Threading.Tasks;

namespace Roufe;

public partial struct Result
{
    /// <summary>
    ///     Creates a result whose success/failure reflects the supplied condition. Opposite of SuccessIf().
    /// </summary>
    public static Result<T, TE> FailureIf<T, TE>(bool isFailure, in T? value, in TE? error)
        => SuccessIf(!isFailure, value, error);

    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of SuccessIf().
    /// </summary>
    public static Result<T, TE> FailureIf<T, TE>(Func<bool> failurePredicate, in T? value, in TE? error)
        => SuccessIf(!failurePredicate(), value, error);
}

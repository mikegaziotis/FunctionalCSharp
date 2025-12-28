using System;
using System.Threading.Tasks;

namespace Roufe;

public partial struct Result
{
    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of SuccessIf().
    /// </summary>
    public static async ValueTask<Result<T, TE>> FailureIf<T, TE>(Func<ValueTask<bool>> failurePredicate, T value, TE error)
    {
        var isFailure = await failurePredicate().ConfigureAwait(DefaultConfigureAwait);
        return SuccessIf(!isFailure, value, error);
    }
}

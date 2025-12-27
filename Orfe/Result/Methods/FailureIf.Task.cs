using System;
using System.Threading.Tasks;

namespace Orfe;

public partial struct Result
{
    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of SuccessIf().
    /// </summary>
    public static async Task<Result<T, TE>> FailureIf<T, TE>(Func<Task<bool>> failurePredicate, T value, TE error)
    {
        var isFailure = await failurePredicate().ConfigureAwait(DefaultConfigureAwait);
        return SuccessIf(!isFailure, value, error);
    }
}

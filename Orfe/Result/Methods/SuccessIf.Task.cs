using System;
using System.Threading.Tasks;

namespace Orfe;

public partial struct Result
{
    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of FailureIf().
    /// </summary>
    public static async Task<Result<T, TE>> SuccessIf<T, TE>(Func<Task<bool>> predicate, T value, TE error)
    {
        var isSuccess = await predicate().ConfigureAwait(DefaultConfigureAwait);
        return SuccessIf(isSuccess, value, error);
    }

    /// <summary>
    ///     Creates a result whose success/failure depends on the supplied predicate. Opposite of FailureIf().
    /// </summary>
    public static async Task<Result<Unit,TE>> SuccessIf<TE>(Func<Task<bool>> predicate, TE error)
    {
        var isSuccess = await predicate().ConfigureAwait(DefaultConfigureAwait);
        return SuccessIf(isSuccess, error);
    }
}


using System;
using System.Threading.Tasks;

namespace Orfe;

public partial struct Result
{

    /// <summary>
    ///     Attempts to execute the supplied function. Returns a Result indicating whether the function executed successfully.
    ///     If the function executed successfully, the result contains its return value.
    /// </summary>
    public static async Task<Result<T, TE>> Try<T, TE>(Func<Task<T>> func, Func<Exception, TE> errorHandler)
    {
        try
        {
            var result = await func().ConfigureAwait(DefaultConfigureAwait);
            return Success<T, TE>(result);
        }
        catch (Exception exc)
        {
            var error = errorHandler(exc);
            return Failure<T, TE>(error);
        }
    }
}

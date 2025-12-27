using System;
using System.Threading.Tasks;

namespace Orfe;

public partial struct Result
{

    public static Result<T, TE> Try<T, TE>(Func<T> func, Func<Exception, TE> errorHandler)
    {
        try
        {
            return Success<T, TE>(func());
        }
        catch (Exception exc)
        {
            var error = errorHandler(exc);
            return Failure<T, TE>(error);
        }
    }

    /// <summary>
    ///     Attempts to execute the supplied action. Returns a UnitResult indicating whether the action executed successfully.
    /// </summary>
    public static Result<Unit,TE> Try<TE>(Action action, Func<Exception, TE> errorHandler)
    {
      try
      {
        action();
        return Success<Unit,TE>(Unit.Value);
      }
      catch (Exception exc)
      {
        var error = errorHandler(exc);
        return Failure<Unit,TE>(error);
      }
    }


    /// <summary>
    ///     Attempts to execute the supplied action. Returns a UnitResult indicating whether the action executed successfully.
    /// </summary>
    public static async Task<Result<Unit,TE>> Try<TE>(Func<Task> action, Func<Exception, TE> errorHandler)
    {
      try
      {
          await action().ConfigureAwait(DefaultConfigureAwait);
          return Success<Unit,TE>(Unit.Value);
      }
      catch (Exception exc)
      {
          var error = errorHandler(exc);
          return Failure<Unit,TE>(error);
      }
    }

    /// <summary>
    ///     Attempts to execute the supplied action. Returns a UnitResult indicating whether the action executed successfully.
    /// </summary>
    public static async ValueTask<Result<Unit,TE>> Try<TE>(Func<ValueTask> action, Func<Exception, TE> errorHandler)
    {
        try
        {
            await action().ConfigureAwait(DefaultConfigureAwait);
            return Success<Unit,TE>(Unit.Value);
        }
        catch (Exception exc)
        {
            var error = errorHandler(exc);
            return Failure<Unit,TE>(error);
        }
    }
}

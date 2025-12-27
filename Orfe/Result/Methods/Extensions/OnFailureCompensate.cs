using System;

namespace Orfe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        public Result<T, TE> OnFailureCompensate(Func<Result<T, TE>> func)
            =>  result.IsFailure
                ? func()
                : result;

        public Result<T, TE> OnFailureCompensate(Func<TE, Result<T, TE>> func)
            => result.IsFailure
                ? func(result.Error)
                : result;
    }
}

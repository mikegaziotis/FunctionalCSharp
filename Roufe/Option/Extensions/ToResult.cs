using System;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)
    {
        public Result<T,string> ToResult(string errorMessage)
            => option.HasNoValue
                ? Result.Failure<T,string>(errorMessage)
                : Result.Success<T,string>(option.GetValueOrThrow());

        public Result<T, TE> ToResult<TE>(TE error)
            => option.HasNoValue
                ? Result.Failure<T, TE>(error)
                : Result.Success<T, TE>(option.GetValueOrThrow());

        public Result<T, TE> ToResult<TE>(Func<TE> errorFunc)
            => option.HasNoValue
                ? Result.Failure<T, TE>(errorFunc())
                : Result.Success<T, TE>(option.GetValueOrThrow());
    }
}

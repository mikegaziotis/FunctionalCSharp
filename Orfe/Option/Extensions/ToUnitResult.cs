using System;

namespace Orfe;

public static partial class OptionExtensions
{
    /// <param name="option"></param>
    /// <typeparam name="TE"></typeparam>
    extension<TE>(in Option<TE> option)
    {
        /// <summary>
        /// Converts an Option&lt;TE&gt; to a Result&lt;Unit, TE&gt;.
        /// Returns Success(Unit) if the Option has no value, otherwise returns Failure with the contained error.
        /// </summary>
        /// <returns></returns>
        public Result<Unit,TE> ToUnitResult()
            => option.HasValue
                    ? Result.Failure<Unit,TE>(option.Value)
                    : Result.Success<Unit,TE>(Unit.Value);

        public Result<Unit,TTe> ToUnitResult<TTe>(TTe error)
            => option.HasNoValue
                ? Result.Failure<Unit,TTe>(error)
                : Result.Success<Unit,TTe>(Unit.Value);

        public Result<Unit,TTe> ToUnitResult<TTe>(Func<TTe> errorFunc)
            => option.HasNoValue
                ? Result.Failure<Unit,TTe>(errorFunc())
                : Result.Success<Unit,TTe>(Unit.Value);
    }
}

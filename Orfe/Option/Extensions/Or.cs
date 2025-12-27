using System;

namespace Orfe;

public static partial class OptionExtensions
{
    /// <param name="option"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(in Option<T> option)
    {
        /// <summary>
        ///     Creates a new <see cref="Option{T}" /> if <paramref name="option" /> is empty, using the result of the supplied
        ///     <paramref name="fallbackOperation" />, otherwise it returns <paramref name="option" />
        /// </summary>
        /// <param name="fallbackOperation"></param>
        /// <returns></returns>
        public Option<T> Or(Func<T> fallbackOperation)
            => option.HasNoValue
                ? fallbackOperation()
                : option;

        /// <summary>
        ///     Returns <paramref name="fallback" /> if <paramref name="option" /> is empty, otherwise it returns
        ///     <paramref name="option" />
        /// </summary>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public Option<T> Or(Option<T> fallback)
            => option.HasNoValue
                ? fallback
                : option;


        /// <summary>
        ///     Returns the result of <paramref name="fallbackOperation" /> if <paramref name="option" /> is empty, otherwise it
        ///     returns <paramref name="option" />
        /// </summary>
        /// <param name="fallbackOperation"></param>
        /// <returns></returns>
        public Option<T> Or(Func<Option<T>> fallbackOperation)
            => option.HasNoValue
                ? fallbackOperation()
                : option;
    }
}

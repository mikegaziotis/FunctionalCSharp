using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     This method should be used in linq queries. We recommend using Map method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<Result<TK,TE>> Select<T, TK,TE>(this Result<T,TE> result, Func<T, Task<TK>> selector)
        => await result.Map(selector).ConfigureAwait(DefaultConfigureAwait);

    extension<T, TK, TE>(Task<Result<T,TE>> result)
    {
        /// <summary>
        ///     This method should be used in linq queries. We recommend using Map method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<Result<TK,TE>> Select(Func<T, TK> selector)
            => await result.Map(selector).ConfigureAwait(DefaultConfigureAwait);

        /// <summary>
        ///     This method should be used in linq queries. We recommend using Map method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<Result<TK,TE>> Select(Func<T, Task<TK>> selector)
            => await result.Map(selector).ConfigureAwait(DefaultConfigureAwait);
    }
}

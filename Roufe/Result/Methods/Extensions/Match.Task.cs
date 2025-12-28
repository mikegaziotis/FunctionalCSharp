using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class AsyncResultExtensionsLeftOperand
{


    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        public async Task Match(Func<T, Task> onSuccess, Func<TE, Task> onFailure)
            => await (await resultTask.ConfigureAwait(DefaultConfigureAwait))
                .Match(onSuccess, onFailure).ConfigureAwait(DefaultConfigureAwait);

        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        public async Task<TK> Match<TK>(Func<T, Task<TK>> onSuccess, Func<TE, Task<TK>> onFailure)
            => await (await resultTask.ConfigureAwait(DefaultConfigureAwait))
                .Match(onSuccess, onFailure).ConfigureAwait(DefaultConfigureAwait);

        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        public async Task Match(Action<T> onSuccess, Action<TE> onFailure)
            => (await resultTask.ConfigureAwait(DefaultConfigureAwait)).Match(onSuccess, onFailure);

        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        public async Task<TK> Match<TK>( Func<T, TK> onSuccess, Func<TE, TK> onFailure)
            => (await resultTask.ConfigureAwait(DefaultConfigureAwait)).Match(onSuccess, onFailure);
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        public Task Match(Func<T, Task> onSuccess, Func<TE, Task> onFailure)
            =>  result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);

        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        public Task<TK> Match<TK>(Func<T, Task<TK>> onSuccess, Func<TE, Task<TK>> onFailure)
            => result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);
    }

}

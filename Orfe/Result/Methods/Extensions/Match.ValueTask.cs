using System;
using System.Threading.Tasks;

namespace Orfe.ValueTasks;

public static partial class AsyncResultExtensionsLeftOperand
{


    extension<T, TE>(ValueTask<Result<T, TE>> resultValueTask)
    {
        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        public async ValueTask Match(Func<T, ValueTask> onSuccess, Func<TE, ValueTask> onFailure)
            => await (await resultValueTask.ConfigureAwait(DefaultConfigureAwait))
                .Match(onSuccess, onFailure).ConfigureAwait(DefaultConfigureAwait);

        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        public async ValueTask<TK> Match<TK>(Func<T, ValueTask<TK>> onSuccess, Func<TE, ValueTask<TK>> onFailure)
            => await (await resultValueTask.ConfigureAwait(DefaultConfigureAwait))
                .Match(onSuccess, onFailure).ConfigureAwait(DefaultConfigureAwait);

        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        public async ValueTask Match(Action<T> onSuccess, Action<TE> onFailure)
            => (await resultValueTask.ConfigureAwait(DefaultConfigureAwait)).Match(onSuccess, onFailure);

        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        public async ValueTask<TK> Match<TK>( Func<T, TK> onSuccess, Func<TE, TK> onFailure)
            => (await resultValueTask.ConfigureAwait(DefaultConfigureAwait)).Match(onSuccess, onFailure);
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        ///     Invokes the given <paramref name="onSuccess"/> action if the calling Result is a success. Otherwise, it invokes the given <paramref name="onFailure"/> action.
        /// </summary>
        public ValueTask Match(Func<T, ValueTask> onSuccess, Func<TE, ValueTask> onFailure)
            =>  result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);

        /// <summary>
        ///     Returns the result of the given <paramref name="onSuccess"/> function if the calling Result is a success. Otherwise, it returns the result of the given <paramref name="onFailure"/> function.
        /// </summary>
        public ValueTask<TK> Match<TK>(Func<T, ValueTask<TK>> onSuccess, Func<TE, ValueTask<TK>> onFailure)
            => result.IsSuccess
                ? onSuccess(result.Value)
                : onFailure(result.Error);
    }

}

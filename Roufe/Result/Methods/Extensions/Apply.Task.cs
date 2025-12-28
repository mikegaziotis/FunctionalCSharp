using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T, TE>> resultTask)
    {
        /// <summary>
        /// Async variant: Applies a Task Result-wrapped function to a Task Result-wrapped value.
        /// </summary>
        public async Task<Result<TR, TE>> Apply<TR>(Task<Result<Func<T, TR>, TE>> funcTask)
        {
            ArgumentNullException.ThrowIfNull(funcTask);

            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);

            return result.Apply(funcResult);
        }
    }

    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        /// Async variant: Applies a Task Result-wrapped function to a synchronous Result value.
        /// </summary>
        public async Task<Result<TR, TE>> Apply<TR>(Task<Result<Func<T, TR>, TE>> funcTask)
        {
            ArgumentNullException.ThrowIfNull(funcTask);

            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Apply(funcResult);
        }
    }

    extension<T, TE, TR>(Task<Result<Func<T, TR>, TE>> funcTask)
    {
        /// <summary>
        /// Async variant: Applies this Task Result-wrapped function to a Result value.
        /// </summary>
        public async Task<Result<TR, TE>> Apply(Result<T, TE> result)
        {
            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return funcResult.Apply(result);
        }

        /// <summary>
        /// Async variant: Applies this Task Result-wrapped function to a Task Result value.
        /// </summary>
        public async Task<Result<TR, TE>> Apply(Task<Result<T, TE>> resultTask)
        {
            ArgumentNullException.ThrowIfNull(resultTask);

            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return funcResult.Apply(result);
        }
    }

    /// <summary>
    /// Async variant: Applies multiple Task Result values to a multi-parameter function.
    /// </summary>
    public static async Task<Result<TR, TE>> Apply<T1, T2, TR, TE>(
        this Task<Result<T1, TE>> result1Task,
        Task<Result<T2, TE>> result2Task,
        Func<T1, T2, TR> func)
    {
        ArgumentNullException.ThrowIfNull(result2Task);
        ArgumentNullException.ThrowIfNull(func);

        var result1 = await result1Task.ConfigureAwait(DefaultConfigureAwait);
        var result2 = await result2Task.ConfigureAwait(DefaultConfigureAwait);

        return result1.Apply(result2, func);
    }

    /// <summary>
    /// Async variant: Applies three Task Result values to a three-parameter function.
    /// </summary>
    public static async Task<Result<TR, TE>> Apply<T1, T2, T3, TR, TE>(
        this Task<Result<T1, TE>> result1Task,
        Task<Result<T2, TE>> result2Task,
        Task<Result<T3, TE>> result3Task,
        Func<T1, T2, T3, TR> func)
    {
        ArgumentNullException.ThrowIfNull(result2Task);
        ArgumentNullException.ThrowIfNull(result3Task);
        ArgumentNullException.ThrowIfNull(func);

        var result1 = await result1Task.ConfigureAwait(DefaultConfigureAwait);
        var result2 = await result2Task.ConfigureAwait(DefaultConfigureAwait);
        var result3 = await result3Task.ConfigureAwait(DefaultConfigureAwait);

        return result1.Apply(result2, result3, func);
    }

    /// <summary>
    /// Async variant: Applies four Task Result values to a four-parameter function.
    /// </summary>
    public static async Task<Result<TR, TE>> Apply<T1, T2, T3, T4, TR, TE>(
        this Task<Result<T1, TE>> result1Task,
        Task<Result<T2, TE>> result2Task,
        Task<Result<T3, TE>> result3Task,
        Task<Result<T4, TE>> result4Task,
        Func<T1, T2, T3, T4, TR> func)
    {
        ArgumentNullException.ThrowIfNull(result2Task);
        ArgumentNullException.ThrowIfNull(result3Task);
        ArgumentNullException.ThrowIfNull(result4Task);
        ArgumentNullException.ThrowIfNull(func);

        var result1 = await result1Task.ConfigureAwait(DefaultConfigureAwait);
        var result2 = await result2Task.ConfigureAwait(DefaultConfigureAwait);
        var result3 = await result3Task.ConfigureAwait(DefaultConfigureAwait);
        var result4 = await result4Task.ConfigureAwait(DefaultConfigureAwait);

        return result1.Apply(result2, result3, result4, func);
    }
}


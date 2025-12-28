using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T, TE>> resultTask)
    {
        /// <summary>
        /// ValueTask variant: Applies a ValueTask Result-wrapped function to a ValueTask Result-wrapped value.
        /// </summary>
        public async ValueTask<Result<TR, TE>> Apply<TR>(ValueTask<Result<Func<T, TR>, TE>> funcTask)
        {
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);

            return result.Apply(funcResult);
        }
    }

    extension<T, TE, TR>(Result<T, TE> result)
    {
        /// <summary>
        /// ValueTask variant: Applies a ValueTask Result-wrapped function to a synchronous Result value.
        /// </summary>
        public async ValueTask<Result<TR, TE>> Apply(ValueTask<Result<Func<T, TR>, TE>> funcTask)
        {

            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return result.Apply(funcResult);
        }
    }

    extension<T, TE, TR>(ValueTask<Result<Func<T, TR>, TE>> funcTask)
    {
        /// <summary>
        /// ValueTask variant: Applies this ValueTask Result-wrapped function to a Result value.
        /// </summary>
        public async ValueTask<Result<TR, TE>> Apply(Result<T, TE> result)
        {
            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return funcResult.Apply(result);
        }

        /// <summary>
        /// ValueTask variant: Applies this ValueTask Result-wrapped function to a ValueTask Result value.
        /// </summary>
        public async ValueTask<Result<TR, TE>> Apply(ValueTask<Result<T, TE>> resultTask)
        {
            var funcResult = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);

            return funcResult.Apply(result);
        }
    }

    /// <summary>
    /// ValueTask variant: Applies multiple ValueTask Result values to a multi-parameter function.
    /// </summary>
    public static async ValueTask<Result<TR, TE>> Apply<T1, T2, TR, TE>(
        this ValueTask<Result<T1, TE>> result1Task,
        ValueTask<Result<T2, TE>> result2Task,
        Func<T1, T2, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        var result1 = await result1Task.ConfigureAwait(DefaultConfigureAwait);
        var result2 = await result2Task.ConfigureAwait(DefaultConfigureAwait);

        return result1.Apply(result2, func);
    }

    /// <summary>
    /// ValueTask variant: Applies three ValueTask Result values to a three-parameter function.
    /// </summary>
    public static async ValueTask<Result<TR, TE>> Apply<T1, T2, T3, TR, TE>(
        this ValueTask<Result<T1, TE>> result1Task,
        ValueTask<Result<T2, TE>> result2Task,
        ValueTask<Result<T3, TE>> result3Task,
        Func<T1, T2, T3, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        var result1 = await result1Task.ConfigureAwait(DefaultConfigureAwait);
        var result2 = await result2Task.ConfigureAwait(DefaultConfigureAwait);
        var result3 = await result3Task.ConfigureAwait(DefaultConfigureAwait);

        return result1.Apply(result2, result3, func);
    }

    /// <summary>
    /// ValueTask variant: Applies four ValueTask Result values to a four-parameter function.
    /// </summary>
    public static async ValueTask<Result<TR, TE>> Apply<T1, T2, T3, T4, TR, TE>(
        this ValueTask<Result<T1, TE>> result1Task,
        ValueTask<Result<T2, TE>> result2Task,
        ValueTask<Result<T3, TE>> result3Task,
        ValueTask<Result<T4, TE>> result4Task,
        Func<T1, T2, T3, T4, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        var result1 = await result1Task.ConfigureAwait(DefaultConfigureAwait);
        var result2 = await result2Task.ConfigureAwait(DefaultConfigureAwait);
        var result3 = await result3Task.ConfigureAwait(DefaultConfigureAwait);
        var result4 = await result4Task.ConfigureAwait(DefaultConfigureAwait);

        return result1.Apply(result2, result3, result4, func);
    }
}


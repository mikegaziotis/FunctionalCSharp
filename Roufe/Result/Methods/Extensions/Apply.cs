using System;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Result<T, TE> result)
    {
        /// <summary>
        /// Applies a Result-wrapped function to a Result-wrapped value.
        /// This is the applicative functor operation for Result.
        /// If either the function or the value is failure, returns the first failure encountered.
        /// </summary>
        /// <example>
        /// Result&lt;Func&lt;int, string&gt;, string&gt; funcResult = Result.Success&lt;Func&lt;int, string&gt;, string&gt;(x => x.ToString());
        /// Result&lt;int, string&gt; valueResult = Result.Success&lt;int, string&gt;(42);
        /// Result&lt;string, string&gt; applied = valueResult.Apply(funcResult);
        /// // applied.Value = "42"
        /// </example>
        public Result<TR, TE> Apply<TR>(Result<Func<T, TR>, TE> funcResult)
        {

            if (funcResult.IsFailure)
                return Result.Failure<TR, TE>(funcResult.Error);

            if (result.IsFailure)
                return Result.Failure<TR, TE>(result.Error);

            return Result.Success<TR, TE>(funcResult.Value(result.Value));
        }
    }

    extension<T, TE, TR>(Result<Func<T, TR>, TE> funcResult)
    {
        /// <summary>
        /// Applies this Result-wrapped function to a Result-wrapped value.
        /// Convenience overload that reverses the call order.
        /// </summary>
        public Result<TR, TE> Apply(Result<T, TE> result)
        {
            if (funcResult.IsFailure)
                return Result.Failure<TR, TE>(funcResult.Error);

            if (result.IsFailure)
                return Result.Failure<TR, TE>(result.Error);

            return Result.Success<TR, TE>(funcResult.Value(result.Value));
        }
    }

    /// <summary>
    /// Lifts a regular function into a Result context.
    /// This is the "pure" operation for Result applicative.
    /// </summary>
    public static Result<Func<T, TR>, TE> Pure<T, TR, TE>(Func<T, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return Result.Success<Func<T, TR>, TE>(func);
    }

    /// <summary>
    /// Applies multiple Result values to a multi-parameter function.
    /// All Results must be successful for the operation to succeed.
    /// </summary>
    public static Result<TR, TE> Apply<T1, T2, TR, TE>(
        this Result<T1, TE> result1,
        Result<T2, TE> result2,
        Func<T1, T2, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        if (result1.IsFailure)
            return Result.Failure<TR, TE>(result1.Error);

        if (result2.IsFailure)
            return Result.Failure<TR, TE>(result2.Error);

        return Result.Success<TR, TE>(func(result1.Value,result2.Value));
    }

    /// <summary>
    /// Applies three Result values to a three-parameter function.
    /// </summary>
    public static Result<TR, TE> Apply<T1, T2, T3, TR, TE>(
        this Result<T1, TE> result1,
        Result<T2, TE> result2,
        Result<T3, TE> result3,
        Func<T1, T2, T3, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        if (result1.IsFailure)
            return Result.Failure<TR, TE>(result1.Error);

        if (result2.IsFailure)
            return Result.Failure<TR, TE>(result2.Error);

        if (result3.IsFailure)
            return Result.Failure<TR, TE>(result3.Error);

        return Result.Success<TR, TE>(func(result1.Value, result2.Value, result3.Value));
    }

    /// <summary>
    /// Applies four Result values to a four-parameter function.
    /// </summary>
    public static Result<TR, TE> Apply<T1, T2, T3, T4, TR, TE>(
        this Result<T1, TE> result1,
        Result<T2, TE> result2,
        Result<T3, TE> result3,
        Result<T4, TE> result4,
        Func<T1, T2, T3, T4, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        if (result1.IsFailure)
            return Result.Failure<TR, TE>(result1.Error);

        if (result2.IsFailure)
            return Result.Failure<TR, TE>(result2.Error);

        if (result3.IsFailure)
            return Result.Failure<TR, TE>(result3.Error);

        if (result4.IsFailure)
            return Result.Failure<TR, TE>(result4.Error);

        return Result.Success<TR, TE>(func(result1.Value, result2.Value, result3.Value, result4.Value));
    }
}


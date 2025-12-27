using System;

namespace Orfe;

public static partial class ResultExtensions
{
    /// <summary>
    ///     Returns a new failure result if the result is null. Otherwise, returns the starting result.
    /// </summary>
    public static Result<T, TE> EnsureNotNull<T, TE>(this Result<T?, TE> result, TE error) where T : class
        => result.Ensure(value => value is not null, error).Map(value => value!);

    /// <summary>
    ///     Returns a new failure result if the result is null. Otherwise, returns the starting result.
    /// </summary>
    public static Result<T, TE> EnsureNotNull<T, TE>(this Result<T?, TE> result, TE error) where T : struct
        => result.Ensure(value => value is not null, error).Map(value => value!.Value);

    /// <summary>
    ///     Returns a new failure result if the result is null. Otherwise, returns the starting result.
    /// </summary>
    public static Result<T, TE> EnsureNotNull<T, TE>(this Result<T?, TE> result, Func<TE> errorFactory) where T : class
        => result.Ensure(value => value is not null, _ => errorFactory()).Map(value => value!);

    /// <summary>
    ///     Returns a new failure result if the result is null. Otherwise, returns the starting result.
    /// </summary>
    public static Result<T, TE> EnsureNotNull<T, TE>(this Result<T?, TE> result, Func<TE> errorFactory) where T : struct
        => result.Ensure(value => value is not null, _ => errorFactory()).Map(value => value!.Value);

}

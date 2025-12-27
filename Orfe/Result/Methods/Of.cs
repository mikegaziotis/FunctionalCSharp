using System;
using System.Threading.Tasks;

namespace Orfe;

public partial struct Result
{
    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> containing the given value.
    /// </summary>
    public static Result<T, TE> Of<T, TE>(T value) where T : notnull
        => new(false, default, value);


    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> containing the given value from a <see cref="Func{T}" />.
    /// </summary>
    public static Result<T, TE> Of<T, TE>(Func<T> func) where T : notnull
        =>  new(false, default, func());


    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> containing the given async value.
    /// </summary>
    public static async Task<Result<T, TE>> Of<T, TE>(Task<T> valueTask) where T : notnull
        => new(false, default, await valueTask.ConfigureAwait(DefaultConfigureAwait));


    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> containing the given value from an async <see cref="Func{T}" />.
    /// </summary>
    public static async Task<Result<T, TE>> Of<T, TE>(Func<Task<T>> valueTaskFunc) where T : notnull
        => new(false, default, await valueTaskFunc().ConfigureAwait(DefaultConfigureAwait));

}

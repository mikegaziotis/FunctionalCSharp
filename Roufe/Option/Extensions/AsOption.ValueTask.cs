using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class OptionExtensions
{
    // ReSharper disable ConvertNullableToShortForm
    /// <summary>
    /// Converts the <see cref="Nullable"/> struct to a <see cref="Option{T}"/>.
    /// </summary>
    /// <returns>Returns the <see cref="Option{T}"/> equivalent to the <see cref="Nullable{T}"/>.</returns>
    public static async ValueTask<Option<T>> AsOption<T>(this ValueTask<Nullable<T>> nullableTask)
        where T : struct
    {
        var nullable = await nullableTask.ConfigureAwait(DefaultConfigureAwait);
        return nullable.AsOption();
    }

    /// <summary>
    /// Wraps the class instance in a <see cref="Option{T}"/>.
    /// </summary>
    /// <returns>Returns <see cref="Option.None"/> if the class instance is null, otherwise returns <see cref="Option.From{T}(T)"/>.</returns>
    public static async ValueTask<Option<T>> AsOption<T>(this ValueTask<T?> nullableTask)
        where T : class
    {
        var nullable = await nullableTask.ConfigureAwait(DefaultConfigureAwait);
        return nullable.AsOption();
    }
}

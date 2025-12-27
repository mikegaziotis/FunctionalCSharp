using System;

namespace Orfe;

public static partial class OptionExtensions
{
    // ReSharper disable ConvertNullableToShortForm
    /// <summary>
    /// Converts the <see cref="Nullable"/> struct to a <see cref="Option{T}"/>.
    /// </summary>
    /// <returns>Returns the <see cref="Option{T}"/> equivalent to the <see cref="Nullable{T}"/>.</returns>
    public static Option<T> AsOption<T>(ref this Nullable<T> value)
        where T : struct
    {
        return System.Runtime.CompilerServices.Unsafe.As<Nullable<T>, Option<T>>(ref value);
    }

    // ReSharper restore ConvertNullableToShortForm

    /// <summary>
    /// Wraps the class instance in a <see cref="Option{T}"/>.
    /// </summary>
    /// <returns>Returns <see cref="Option.None"/> if the class instance is null, otherwise returns <see cref="Option.From{T}(T)"/>.</returns>
    public static Option<T> AsOption<T>(this T? value)
        where T : class
    {
        return value is not null ? (Option<T>)value! : default;
    }
}

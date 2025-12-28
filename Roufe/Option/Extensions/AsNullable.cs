using System;

namespace Roufe;

public static partial class OptionExtensions
{
    // ReSharper disable ConvertNullableToShortForm
    /// <summary>
    /// Converts the <see cref="Option{T}"/> to a <see cref="Nullable"/> struct.
    /// </summary>
    /// <returns>Returns the <see cref="Nullable{T}"/> equivalent to the <see cref="Option{T}"/>.</returns>

    public static Nullable<T> AsNullable<T>(ref this Option<T> value)
        where T : struct
    {
        return System.Runtime.CompilerServices.Unsafe.As<Option<T>, Nullable<T>>(ref value);
    }
}

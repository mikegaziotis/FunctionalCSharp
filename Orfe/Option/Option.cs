using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Orfe;

[Serializable]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
public readonly partial struct Option<T> : IEquatable<Option<T>>, IEquatable<object>, IOption<T>
{
    private readonly bool _isValueSet;

    private readonly T? _value;

    /// <summary>
    /// Returns the inner value if there's one, otherwise throws an InvalidOperationException with <paramref name="errorMessage"/>
    /// </summary>
    /// <exception cref="InvalidOperationException">Option has no value.</exception>
    public T GetValueOrThrow(string? errorMessage = null)
        => HasNoValue
            ? throw new InvalidOperationException(errorMessage ?? Configuration.NoValueException)
            : _value;

    /// <summary>
    /// Returns the inner value if there's one, otherwise throws a custom exception with <paramref name="exception"/>
    /// </summary>
    /// <exception cref="Exception">Option has no value.</exception>
    public T GetValueOrThrow(Exception exception)
    => HasNoValue
        ? throw exception
        : _value;

    public T GetValueOrDefault(T defaultValue)
    =>  HasNoValue
        ? defaultValue
        : _value;


    public T? GetValueOrDefault()
    => HasNoValue
        ? default
        : _value;


    /// <summary>
    ///  Indicates whether the inner value is present and returns the value if it is.
    /// </summary>
    /// <param name="value">The inner value, if present; otherwise `default`</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        value = _value;
        return _isValueSet;
    }

    /// <summary>
    /// Try to use GetValueOrThrow() or GetValueOrDefault() instead for better explicitness.
    /// </summary>
    public T Value => GetValueOrThrow();

    public static Option<T> None => new Option<T>();

    [MemberNotNullWhen(true, "_value")]
    public bool HasValue => _isValueSet;

    [MemberNotNullWhen(false, "_value")]
    public bool HasNoValue => !HasValue;

    private Option(T? value)
    {
        if (value == null)
        {
            _isValueSet = false;
            _value = default;
            return;
        }

        _isValueSet = true;
        _value = value;
    }

    public static implicit operator Option<T>(T? value)
    {
        if (value is Option<T> m)
        {
            return m;
        }

        return Option.From(value);
    }

    public static implicit operator Option<T>(Option _) => None;

    public static Option<T> From(T? value)
    {
        return new Option<T>(value);
    }

    public static Option<T> From(Func<T?> func)
    {
        var value = func();

        return new Option<T>(value);
    }

    public static async Task<Option<T>> From(Task<T?> valueTask)
    {
        var value = await valueTask.ConfigureAwait(DefaultConfigureAwait);

        return new Option<T>(value);
    }

    public static async Task<Option<T>> From(Func<Task<T?>> valueTaskFunc)
    {
        var value = await valueTaskFunc().ConfigureAwait(DefaultConfigureAwait);

        return new Option<T>(value);
    }

    public static bool operator ==(Option<T> option, T? value)
    {
        if (value is Option<T> optionValue)
            return option.Equals(optionValue);

        if (option.HasNoValue)
            return value == null;

        return option._value.Equals(value);
    }

    public static bool operator !=(Option<T> option, T value)
    {
        return !(option == value);
    }

    public static bool operator ==(Option<T> option, object other)
    {
        return option.Equals(other);
    }

    public static bool operator !=(Option<T> option, object other)
        => !(option == other);

    public static bool operator ==(Option<T> first, Option<T> second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(Option<T> first, Option<T> second)
    {
        return !(first == second);
    }

    public override bool Equals(object? obj)
        => obj switch
        {
            null => false,
            Option<T> otherOption => Equals(otherOption),
            T otherValue => Equals(otherValue),
            _ => false
        };


    public bool Equals(Option<T> other)
    {
        if (HasNoValue && other.HasNoValue)
            return true;

        if (HasNoValue || other.HasNoValue)
            return false;

        return EqualityComparer<T>.Default.Equals(_value, other._value);
    }

    public override int GetHashCode()
        => HasNoValue
            ? 0 :
            _value.GetHashCode();

    public override string ToString()
        => HasNoValue
            ? "No value"
            : _value.ToString() ?? _value.GetType().Name;

}

/// <summary>
/// Non-generic entrypoint for <see cref="Option{T}" /> members
/// </summary>
public readonly struct Option
{
    public static Option None => new();

    /// <summary>
    /// Creates a new <see cref="Option{T}" /> from the provided <paramref name="value"/>
    /// </summary>
    public static Option<T> From<T>(T? value) => Option<T>.From(value);

    /// <summary>
    /// Creates a new <see cref="Option{T}" /> from the provided <paramref name="func"/>
    /// </summary>
    public static Option<T> From<T>(Func<T?> func) => Option<T>.From(func);

    /// <summary>
    /// Creates a new <see cref="Option{T}" /> from the provided <paramref name="valueTask"/>
    /// </summary>
    public static Task<Option<T>> From<T>(Task<T?> valueTask) => Option<T>.From(valueTask);

    /// <summary>
    /// Creates a new <see cref="Option{T}" /> from the provided <paramref name="valueTaskFunc"/>
    /// </summary>
    public static Task<Option<T>> From<T>(Func<Task<T?>> valueTaskFunc) => Option<T>.From(valueTaskFunc);
}

/// <summary>
/// Useful in scenarios where you need to determine if a value is Option or not
/// </summary>
public interface IOption<out T>
{
    T Value { get; }
    bool HasValue { get; }
    bool HasNoValue { get; }
}

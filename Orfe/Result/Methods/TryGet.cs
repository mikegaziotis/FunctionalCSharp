using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Orfe;

partial struct Result<T, TE>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        value = _value.Match(x=> x, () => default!);
        return IsSuccess;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(out Option<T> value)
    {
        value = _value;
        return IsSuccess;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetError([NotNullWhen(true)] out TE? error)
    {
        error = _error.Match(x=> x, () => default!);
        return IsFailure;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetError(out Option<TE> error)
    {
        error = _error;
        return IsFailure;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue([NotNullWhen(true)] out T? value, [NotNullWhen(false)] out TE? error)
    {
        value = IsSuccess ? _value.Match(x=> x, () => default!) : default!;
        error = IsSuccess ? default! : _error.Match(x=> x, () => default!);
        return IsSuccess;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue(out Option<T> value, out Option<TE> error)
    {
        value = _value;
        error = _error;
        return IsSuccess;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetError([NotNullWhen(true), MaybeNullWhen(false)] out TE error, [NotNullWhen(false), MaybeNullWhen(true)] out T value)
    {
        value = IsFailure ? default! : _value.Match(x=> x, () => default!);
        error = IsFailure ? _error.Match(x=> x, () => default!) : default!;
        return IsFailure;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetError(out Option<TE> error, out Option<T> value)
    {
        value = _value;
        error = _error;
        return IsFailure;
    }
}

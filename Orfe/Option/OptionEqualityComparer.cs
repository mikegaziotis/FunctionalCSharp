using System.Collections.Generic;

namespace Orfe;

public class OptionEqualityComparer<T>(IEqualityComparer<T>? equalityComparer = null) : IEqualityComparer<Option<T>>
{
    private readonly IEqualityComparer<T>? _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

    public bool Equals(Option<T> x, Option<T> y)
    {
        if (x.HasNoValue && y.HasNoValue)
            return true;

        if (_equalityComparer is null)
            return false;

        return x.HasValue && y.HasValue && _equalityComparer.Equals(x.Value, y.Value);
    }

    public int GetHashCode(Option<T> obj)
    {
        if (_equalityComparer is null)
            return 0;

        return obj.HasNoValue ? 0 : _equalityComparer.GetHashCode(obj.Value!);
    }
}

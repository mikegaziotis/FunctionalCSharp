using System;
using System.Collections.Generic;

namespace Orfe;

public static partial class OptionExtensions
{
    public static List<T> ToList<T>(in this Option<T> option)
        => option.GetValueOrDefault(value => [value], new List<T>())
           ?? throw new InvalidOperationException();
}

using System.Collections.Generic;

namespace Roufe;

public static partial class OptionExtensions
{

    public static Option<TV> TryFind<TK, TV>(this IReadOnlyDictionary<TK, TV> dict, TK key)
        => dict.TryGetValue(key, out var find)
            ? find
            : Option<TV>.None;

}

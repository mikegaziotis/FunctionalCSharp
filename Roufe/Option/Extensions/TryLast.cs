using System;
using System.Collections.Generic;
using System.Linq;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public Option<T> TryLast()
        {
            source = source as ICollection<T> ?? source.ToList();

            return source.Any()
                ? Option<T>.From(source.Last())
                : Option<T>.None;
        }

        public Option<T> TryLast(Func<T, bool> predicate)
        {
            var last = source.LastOrDefault(predicate);
            return last != null
                ? Option<T>.From(last)
                : Option<T>.None;
        }
    }
}

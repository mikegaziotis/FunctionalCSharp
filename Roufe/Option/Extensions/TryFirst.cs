using System;
using System.Collections.Generic;
using System.Linq;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public Option<T> TryFirst()
        {
            source = source as ICollection<T> ?? source.ToList();

            return source.Any()
                ? Option<T>.From(source.First())
                : Option<T>.None;
        }

        public Option<T> TryFirst(Func<T, bool> predicate)
        {
            var firstOrEmpty = source.Where(predicate).Take(1).ToList();
            return firstOrEmpty.Count != 0
                ? Option<T>.From(firstOrEmpty[0])
                : Option<T>.None;
        }
    }
}

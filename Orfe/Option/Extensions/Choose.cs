using System;
using System.Collections.Generic;

namespace Orfe;

public static partial class OptionExtensions
{
    extension<T>(IEnumerable<Option<T>> source)
    {
        public IEnumerable<TU> Choose<TU>(Func<T, TU> selector)
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.HasValue) yield return selector(item.GetValueOrThrow());
            }
        }

        public IEnumerable<T> Choose()
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.HasValue) yield return item.GetValueOrThrow();
            }
        }
    }
}

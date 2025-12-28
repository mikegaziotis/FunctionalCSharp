using System;
using System.Globalization;

namespace Roufe
{
    public static class NumberExtensions
    {
        // Convert a value implementing IConvertible to double using invariant culture.
        private static double ToDouble(IConvertible value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        // Returns true if value > other
        extension<T>(T value) where T : IConvertible
        {
            public bool GreaterThan<TU>(TU other) where TU : IConvertible
            {
                var a = ToDouble(value);
                var b = ToDouble(other);
                if (double.IsNaN(a) || double.IsNaN(b)) return false;
                return a > b;
            }

            public bool GreaterThanOrEqualTo<TU>(TU other) where TU : IConvertible
            {
                var a = ToDouble(value);
                var b = ToDouble(other);
                if (double.IsNaN(a) || double.IsNaN(b)) return false;
                return a >= b;
            }

            public bool LessThanOrEqualTo<TU>(TU other) where TU : IConvertible
            {
                var a = ToDouble(value);
                var b = ToDouble(other);
                if (double.IsNaN(a) || double.IsNaN(b)) return false;
                return a <= b;
            }

            public bool IsBetween<TU, TV>(TU min, TV max, InclusionType inclusionType = InclusionType.InclusiveBothEnds) where TU : IConvertible
                where TV : IConvertible
            {
                var v = ToDouble(value);
                var a = ToDouble(min);
                var b = ToDouble(max);

                if (double.IsNaN(v) || double.IsNaN(a) || double.IsNaN(b)) return false;

                if (a > b) // swap to allow callers to pass bounds in any order
                {
                    (a, b) = (b, a);
                }

                return inclusionType switch
                {
                    InclusionType.InclusiveBothEnds => v >= a && v <= b,
                    InclusionType.InclusiveLowerExclusiveUpper => v >= a && v < b,
                    InclusionType.ExclusiveLoweInclusiveUpper => v > a && v <= b,
                    InclusionType.ExclusiveBothEnds => v > a && v < b,
                    _ => throw new ArgumentOutOfRangeException(nameof(inclusionType), "Invalid inclusion type specified.")
                };
            }
        }


        public enum InclusionType
        {
            InclusiveBothEnds = 1,
            InclusiveLowerExclusiveUpper = 2,
            ExclusiveLoweInclusiveUpper = 3,
            ExclusiveBothEnds = 4
        }
    }
}

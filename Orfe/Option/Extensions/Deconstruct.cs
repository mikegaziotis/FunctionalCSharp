namespace Orfe;

public static partial class OptionExtensions
{
    public static void Deconstruct<T>(in this Option<T> result, out bool hasValue, out T? value)
    {
        hasValue = result.HasValue;
        value = result.GetValueOrDefault();
    }
}

using System;

namespace Roufe;

public static partial class OptionExtensions
{
    /// <summary>
    ///     Executes the given <paramref name="action" /> if the <paramref name="option" /> has a value
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void Execute<T>(in this Option<T> option, Action<T> action)
    {
        if (option.HasNoValue)
            return;

        action(option.GetValueOrThrow());
    }
}

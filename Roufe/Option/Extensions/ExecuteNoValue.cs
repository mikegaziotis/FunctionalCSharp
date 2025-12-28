using System;

namespace Roufe;

public static partial class OptionExtensions
{
    /// <summary>
    ///     Executes the given <paramref name="action" /> if the <paramref name="option" /> has no value
    /// </summary>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ExecuteNoValue<T>(in this Option<T> option, Action action)
    {
        if (option.HasValue)
            return;

        action();
    }
}

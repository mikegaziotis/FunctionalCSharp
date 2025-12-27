namespace Orfe;

public static partial class ResultExtensions
{
    /// <summary>
    /// Converts a result to an Option. If it's Success it returns Some and if it's a Failure it returns None
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <returns></returns>
    public static Option<T> AsOption<T,TE>(this Result<T,TE> result)
    {
        return result.IsSuccess
            ? Option<T>.From(result.Value)
            : Option<T>.None;
    }
}


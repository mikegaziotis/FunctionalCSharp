using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    /// <summary>
    /// Converts a result to an Option. If it's Success it returns Some and if it's a Failure it returns None
    /// </summary>
    /// <param name="resultTask"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <returns></returns>
    public static async ValueTask<Option<T>> AsOption<T,TE>(this ValueTask<Result<T,TE>> resultTask)
    {
        var result = await resultTask.ConfigureAwait(DefaultConfigureAwait);
        return result.IsSuccess
            ? Option<T>.From(result.Value)
            : Option<T>.None;
    }
}

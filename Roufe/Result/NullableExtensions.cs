
using System.Threading.Tasks;
// ReSharper disable CheckNamespace

namespace Roufe;

public static partial class NullableExtensions
{
    public static Result<T, TE> ToResult<T, TE>(in this T? nullable, TE error) where T : struct
        => !nullable.HasValue
            ? Result.Failure<T, TE>(error)
            : Result.Success<T, TE>(nullable.Value);

    public static Result<T, TE> ToResult<T, TE>(this T? obj, TE error) where T : class
        => obj is null
            ? Result.Failure<T, TE>(error)
            : Result.Success<T, TE>(obj);

    public static async Task<Result<T, TE>> ToResultAsync<T, TE>(this Task<T?> nullableTask, TE errors) where T : struct
    {
        var nullable = await nullableTask.ConfigureAwait(DefaultConfigureAwait);
        return nullable.ToResult(errors);
    }

    public static async Task<Result<T, TE>> ToResultAsync<T, TE>(this Task<T?> nullableTask, TE errors) where T : class
    {
        var nullable = await nullableTask.ConfigureAwait(DefaultConfigureAwait);
        return nullable.ToResult(errors);
    }

    public static async ValueTask<Result<T, TE>> ToResultAsync<T, TE>(this ValueTask<T?> nullableTask, TE errors)
        where T : struct
    {
        var nullable = await nullableTask.ConfigureAwait(DefaultConfigureAwait);
        return nullable.ToResult(errors);
    }

    public static async ValueTask<Result<T, TE>> ToResultAsync<T, TE>(this ValueTask<T?> nullableTask, TE errors)
        where T : class
    {
        var nullable = await nullableTask.ConfigureAwait(DefaultConfigureAwait);
        return nullable.ToResult(errors);
    }
}

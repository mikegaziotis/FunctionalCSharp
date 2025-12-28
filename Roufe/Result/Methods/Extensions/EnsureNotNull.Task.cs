using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TE>(Task<Result<T?, TE>> resultTask) where T : class
    {
        public Task<Result<T, TE>> EnsureNotNull(TE error)
            => resultTask
                .Ensure(value => value is not null, error)
                .Map(value => value!);

        public Task<Result<T, TE>> EnsureNotNull(Func<Task<TE>> errorFactory)
            => resultTask
                .Ensure(value => Task.FromResult(value is not null), _ => errorFactory())
                .Map(value => value!);

        public Task<Result<T, TE>> EnsureNotNull(Func<TE> errorFactory)
            => resultTask
                .Ensure(value => Task.FromResult(value is not null), _ => errorFactory())
                .Map(value => value!);
    }

    extension<T, TE>(Task<Result<T?, TE>> resultTask) where T : struct
    {
        public Task<Result<T, TE>> EnsureNotNull(TE error)
            => resultTask
                .Ensure(value => value is not null, error)
                .Map(value => value!.Value);

        public Task<Result<T, TE>> EnsureNotNull(Func<Task<TE>> errorFactory)
            => resultTask
                .Ensure(value => Task.FromResult(value is not null), _ => errorFactory())
                .Map(value => value!.Value);

        public Task<Result<T, TE>> EnsureNotNull(Func<TE> errorFactory)
            => resultTask
                .Ensure(value => Task.FromResult(value is not null), _ => errorFactory())
                .Map(value => value!.Value);
    }

    public static Task<Result<T, TE>> EnsureNotNull<T, TE>(this Result<T?, TE> result, Func<Task<TE>> errorFactory) where T : class
        => result.Ensure(value => Task.FromResult(value is not null), _ => errorFactory()).Map(value => value!);


    public static Task<Result<T, TE>> EnsureNotNull<T, TE>(this Result<T?, TE> result, Func<Task<TE>> errorFactory) where T : struct
        => result.Ensure(value => Task.FromResult(value is not null), _ => errorFactory()).Map(value => value!.Value);
}

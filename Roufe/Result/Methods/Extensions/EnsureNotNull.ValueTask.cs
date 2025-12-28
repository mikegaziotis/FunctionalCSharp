using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TE>(ValueTask<Result<T?, TE>> resultValueTask) where T : class
    {
        public async ValueTask<Result<T, TE>> EnsureNotNull(TE error)
            => (await resultValueTask.ConfigureAwait(DefaultConfigureAwait))
                .Ensure(value => value is not null, error)
                .Map(value => value!);

        public async ValueTask<Result<T, TE>> EnsureNotNull(Func<ValueTask<TE>> errorFactory)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            var ensuredResult = await result.Ensure(IsNotNull, IsError).ConfigureAwait(DefaultConfigureAwait);
            return ensuredResult.Map(value => value!);

            ValueTask<bool> IsNotNull (T? value)=> (value is not null).AsCompletedValueTask();
            async ValueTask<TE> IsError (T? _)=> await errorFactory().ConfigureAwait(DefaultConfigureAwait);
        }


        public async ValueTask<Result<T, TE>> EnsureNotNull(Func<TE> errorFactory)
        {
            var result = await resultValueTask.ConfigureAwait(DefaultConfigureAwait);
            var ensuredResult = await result.Ensure(IsNotNull, errorFactory()).ConfigureAwait(DefaultConfigureAwait);
            return ensuredResult.Map(value => value!);

            ValueTask<bool> IsNotNull (T? value)=> (value is not null).AsCompletedValueTask();
        }
    }

    extension<T, TE>(ValueTask<Result<T?, TE>> resultValueTask) where T : struct
    {
        public ValueTask<Result<T, TE>> EnsureNotNull(TE error)
            => resultValueTask
                .Ensure(value => value is not null, error)
                .Map(value => value!.Value);

        public ValueTask<Result<T, TE>> EnsureNotNull(Func<ValueTask<TE>> errorFactory)
            => resultValueTask
                .Ensure(value => ValueTask.FromResult(value is not null), _ => errorFactory())
                .Map(value => value!.Value);

        public ValueTask<Result<T, TE>> EnsureNotNull(Func<TE> errorFactory)
            => resultValueTask
                .Ensure(value => ValueTask.FromResult(value is not null), _ => errorFactory())
                .Map(value => value!.Value);
    }

    public static ValueTask<Result<T, TE>> EnsureNotNull<T, TE>(this Result<T?, TE> result, Func<ValueTask<TE>> errorFactory) where T : class
        => result.Ensure(value => ValueTask.FromResult(value is not null), _ => errorFactory()).Map(value => value!);


    public static ValueTask<Result<T, TE>> EnsureNotNull<T, TE>(this Result<T?, TE> result, Func<ValueTask<TE>> errorFactory) where T : struct
        => result.Ensure(value => ValueTask.FromResult(value is not null), _ => errorFactory()).Map(value => value!.Value);
}

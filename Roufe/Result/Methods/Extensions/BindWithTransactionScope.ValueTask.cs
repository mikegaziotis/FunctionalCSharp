using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(ValueTask<Result<T, TE>> self)
    {
        public ValueTask<Result<TK, TE>> BindWithTransactionScope(Func<T, Result<TK, TE>> f)
            => WithTransactionScope(() => self.Bind(f));

        public ValueTask<Result<TK, TE>> BindWithTransactionScope(Func<T, ValueTask<Result<TK, TE>>> f)
            => WithTransactionScope(() => self.Bind(f));
    }

    public static ValueTask<Result<TK, TE>> BindWithTransactionScope<T, TK, TE>(this Result<T, TE> self, Func<T, ValueTask<Result<TK, TE>>> f)
        => WithTransactionScope(() => self.Bind(f));
}


using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    public static Task<Result<TK, TE>> BindWithTransactionScope<T, TK, TE>(this Task<Result<T, TE>> self, Func<T, Task<Result<TK, TE>>> f)
        => WithTransactionScope(() => self.Bind(f));

    public static Task<Result<TK, TE>> BindWithTransactionScope<T, TK, TE>(this Task<Result<T, TE>> self, Func<T, Result<TK, TE>> f)
        => WithTransactionScope(() => self.Bind(f));

    public static Task<Result<TK, TE>> BindWithTransactionScope<T, TK, TE>(this Result<T, TE> self, Func<T, Task<Result<TK, TE>>> f)
        => WithTransactionScope(() => self.Bind(f));
}

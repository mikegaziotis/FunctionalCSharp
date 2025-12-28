using System;

namespace Roufe;

public static partial class ResultExtensions
{
    public static Result<TK, TE> BindWithTransactionScope<T, TK, TE>(this Result<T, TE> self, Func<T, Result<TK, TE>> f)
        => WithTransactionScope(() => self.Bind(f));
}

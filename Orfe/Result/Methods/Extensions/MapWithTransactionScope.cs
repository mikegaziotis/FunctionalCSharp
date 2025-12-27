using System;

namespace Orfe;

public static partial class ResultExtensions
{
    public static Result<TK,TE> MapWithTransactionScope<T, TK, TE>(this Result<T,TE> self, Func<T, TK> f)
        => WithTransactionScope(() => self.Map(f));

}

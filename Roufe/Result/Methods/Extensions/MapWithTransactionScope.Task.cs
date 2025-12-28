using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(Task<Result<T,TE>> self)
    {
        public Task<Result<TK,TE>> MapWithTransactionScope(Func<T, Task<TK>> f)
            => WithTransactionScope(() => self.Map(f));

        public Task<Result<TK,TE>> MapWithTransactionScope(Func<T, TK> f)
            => WithTransactionScope(() => self.Map(f));
    }

    public static Task<Result<TK,TE>> MapWithTransactionScope<T, TK, TE>(this Result<T,TE> self, Func<T, Task<TK>> f)
        => WithTransactionScope(() => self.Map(f));
}

using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class ResultExtensions
{
    extension<T, TK, TE>(ValueTask<Result<T,TE>> self)
    {
        public ValueTask<Result<TK,TE>> MapWithTransactionScope(Func<T, ValueTask<TK>> f)
            => WithTransactionScope(() => self.Map(f));

        public ValueTask<Result<TK,TE>> MapWithTransactionScope(Func<T, TK> f)
            => WithTransactionScope(() => self.Map(f));
    }

    public static ValueTask<Result<TK,TE>> MapWithTransactionScope<T, TK, TE>(this Result<T,TE> self, Func<T, ValueTask<TK>> f)
        => WithTransactionScope(() => self.Map(f));
}

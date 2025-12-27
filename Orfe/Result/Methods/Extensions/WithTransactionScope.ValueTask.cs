using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Orfe.ValueTasks;

public static partial class ResultExtensions
{
    private static async ValueTask<T> WithTransactionScope<T>(Func<ValueTask<T>> f)
        where T : IResult
    {
        using var trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var result = await f().ConfigureAwait(DefaultConfigureAwait);
        if (result.IsSuccess)
        {
            trans.Complete();
        }

        return result;
    }
}

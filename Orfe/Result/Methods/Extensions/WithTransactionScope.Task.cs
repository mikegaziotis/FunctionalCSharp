using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Orfe;

public static partial class ResultExtensions
{


    private static async Task<T> WithTransactionScope<T>(Func<Task<T>> f)
        where T : IResult
    {
        using var trans = new TransactionScope(TransactionScopeOption.Required, TransactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        var result = await f().ConfigureAwait(DefaultConfigureAwait);
        if (result.IsSuccess)
        {
            trans.Complete();
        }

        return result;
    }
}

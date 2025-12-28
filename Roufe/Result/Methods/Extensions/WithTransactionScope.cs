using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Roufe;

public static partial class ResultExtensions
{
    private static readonly TransactionOptions TransactionOptions = new()
    {
        IsolationLevel = IsolationLevel.ReadCommitted,
        Timeout = TransactionManager.DefaultTimeout
    };

    private static T WithTransactionScope<T>(Func<T> f)
        where T : IResult
    {
        using var trans = new TransactionScope(TransactionScopeOption.Required, TransactionOptions, TransactionScopeAsyncFlowOption.Enabled);
        var result = f();
        if (result.IsSuccess)
        {
            trans.Complete();
        }

        return result;
    }
}

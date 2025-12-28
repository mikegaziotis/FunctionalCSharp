using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class OptionExtensions
{
    extension<T>(ValueTask<Option<T>> optionTask)
    {

        public async ValueTask<Result<T, TE>> ToResult<TE>(TE error)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToResult(error);
        }

        public async ValueTask<Result<T, TE>> ToResult<TE>(Func<TE> errorFunc)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToResult(errorFunc);
        }
    }
}

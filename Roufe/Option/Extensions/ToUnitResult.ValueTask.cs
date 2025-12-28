using System;
using System.Threading.Tasks;

namespace Roufe.ValueTasks;

public static partial class OptionExtensions
{
    extension<TE>(ValueTask<Option<TE>> optionTask)
    {
        public async ValueTask<Result<Unit,TE>> ToUnitResult()
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToUnitResult();
        }

        public async ValueTask<Result<Unit,TTe>> ToUnitResult<TTe>(TTe error)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToUnitResult(error);
        }

        public async ValueTask<Result<Unit,TTe>> ToUnitResult<TTe>(Func<TTe> errorFunc)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToUnitResult(errorFunc);
        }
    }
}

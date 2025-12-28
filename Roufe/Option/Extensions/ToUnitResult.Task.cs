using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<TE>(Task<Option<TE>> optionTask)
    {
        public async Task<Result<Unit,TE>> ToUnitResult()
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToUnitResult();
        }

        public async Task<Result<Unit,TTe>> ToUnitResult<TTe>(TTe error)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToUnitResult(error);
        }

        public async Task<Result<Unit,TTe>> ToUnitResult<TTe>(Func<TTe> errorFunc)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToUnitResult(errorFunc);
        }
    }
}

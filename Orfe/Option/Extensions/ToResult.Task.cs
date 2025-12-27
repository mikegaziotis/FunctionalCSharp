using System;
using System.Threading.Tasks;

namespace Orfe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {

        public async Task<Result<T, TE>> ToResult<TE>(TE error)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToResult(error);
        }

        public async Task<Result<T, TE>> ToResult<TE>(Func<TE> errorFunc)
        {
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            return option.ToResult(errorFunc);
        }
    }
}

using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class OptionExtensions
{

    /// <summary>
    /// ValueTask variant: Applies a ValueTask Option-wrapped function to a ValueTask Option-wrapped value.
    /// </summary>
    public static async ValueTask<Option<TR>> Apply<T,TR>(this ValueTask<Option<T>> optionTask, ValueTask<Option<Func<T, TR>>> funcTask)
    {
        var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
        var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);

        return option.Apply(funcOption);
    }

    /// <summary>
    /// ValueTask variant: Applies a ValueTask Option-wrapped function to a synchronous Option value.
    /// </summary>
    public static async ValueTask<Option<TR>> Apply<T, TR>(this Option<T> option, ValueTask<Option<Func<T, TR>>> funcTask)
    {
        var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);
        return option.Apply(funcOption);
    }


    extension<T,TR>(ValueTask<Option<Func<T, TR>>> funcTask)
    {
        /// <summary>
        /// ValueTask variant: Applies this ValueTask Option-wrapped function to an Option value.
        /// </summary>
        public async ValueTask<Option<TR>> Apply(Option<T> option)
        {
            var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return funcOption.Apply(option);
        }

        /// <summary>
        /// ValueTask variant: Applies this ValueTask Option-wrapped function to a ValueTask Option value.
        /// </summary>
        public async ValueTask<Option<TR>> Apply(ValueTask<Option<T>> optionTask)
        {
            var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            return funcOption.Apply(option);
        }
    }

    extension<T1>(ValueTask<Option<T1>> option1Task)
    {
        /// <summary>
        /// ValueTask variant: Applies multiple ValueTask Option values to a multi-parameter function.
        /// </summary>
        public async ValueTask<Option<TR>> Apply<T2, TR>(ValueTask<Option<T2>> option2Task,
            Func<T1, T2, TR> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            var option1 = await option1Task.ConfigureAwait(DefaultConfigureAwait);
            var option2 = await option2Task.ConfigureAwait(DefaultConfigureAwait);

            return option1.Apply(option2, func);
        }

        /// <summary>
        /// ValueTask variant: Applies three ValueTask Option values to a three-parameter function.
        /// </summary>
        public async ValueTask<Option<TR>> Apply<T2, T3, TR>(ValueTask<Option<T2>> option2Task,
            ValueTask<Option<T3>> option3Task,
            Func<T1, T2, T3, TR> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            var option1 = await option1Task.ConfigureAwait(DefaultConfigureAwait);
            var option2 = await option2Task.ConfigureAwait(DefaultConfigureAwait);
            var option3 = await option3Task.ConfigureAwait(DefaultConfigureAwait);

            return option1.Apply(option2, option3, func);
        }

        /// <summary>
        /// ValueTask variant: Applies four ValueTask Option values to a four-parameter function.
        /// </summary>
        public async ValueTask<Option<TR>> Apply<T2, T3, T4, TR>(ValueTask<Option<T2>> option2Task,
            ValueTask<Option<T3>> option3Task,
            ValueTask<Option<T4>> option4Task,
            Func<T1, T2, T3, T4, TR> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            var option1 = await option1Task.ConfigureAwait(DefaultConfigureAwait);
            var option2 = await option2Task.ConfigureAwait(DefaultConfigureAwait);
            var option3 = await option3Task.ConfigureAwait(DefaultConfigureAwait);
            var option4 = await option4Task.ConfigureAwait(DefaultConfigureAwait);

            return option1.Apply(option2, option3, option4, func);
        }
    }
}


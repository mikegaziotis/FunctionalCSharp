using System;
using System.Threading.Tasks;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(Task<Option<T>> optionTask)
    {
        /// <summary>
        /// Async variant: Applies a Task Option-wrapped function to a Task Option-wrapped value.
        /// </summary>
        public async Task<Option<TR>> Apply<TR>(Task<Option<Func<T, TR>>> funcTask)
        {
            ArgumentNullException.ThrowIfNull(funcTask);

            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);
            var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);

            return option.Apply(funcOption);
        }
    }

    extension<T>(Option<T> option)
    {
        /// <summary>
        /// Async variant: Applies a Task Option-wrapped function to a synchronous Option value.
        /// </summary>
        public async Task<Option<TR>> Apply<TR>(Task<Option<Func<T, TR>>> funcTask)
        {
            ArgumentNullException.ThrowIfNull(funcTask);

            var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return option.Apply(funcOption);
        }
    }

    extension<T, TR>(Task<Option<Func<T, TR>>> funcTask)
    {
        /// <summary>
        /// Async variant: Applies this Task Option-wrapped function to an Option value.
        /// </summary>
        public async Task<Option<TR>> Apply(Option<T> option)
        {
            var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            return funcOption.Apply(option);
        }

        /// <summary>
        /// Async variant: Applies this Task Option-wrapped function to a Task Option value.
        /// </summary>
        public async Task<Option<TR>> Apply(Task<Option<T>> optionTask)
        {
            ArgumentNullException.ThrowIfNull(optionTask);

            var funcOption = await funcTask.ConfigureAwait(DefaultConfigureAwait);
            var option = await optionTask.ConfigureAwait(DefaultConfigureAwait);

            return funcOption.Apply(option);
        }
    }

    extension<T1>(Task<Option<T1>> option1Task)
    {
        /// <summary>
        /// Async variant: Applies multiple Task Option values to a multi-parameter function.
        /// </summary>
        public async Task<Option<TR>> Apply<T2, TR>(Task<Option<T2>> option2Task,
            Func<T1, T2, TR> func)
        {
            ArgumentNullException.ThrowIfNull(option2Task);
            ArgumentNullException.ThrowIfNull(func);

            var option1 = await option1Task.ConfigureAwait(DefaultConfigureAwait);
            var option2 = await option2Task.ConfigureAwait(DefaultConfigureAwait);

            return option1.Apply(option2, func);
        }

        /// <summary>
        /// Async variant: Applies three Task Option values to a three-parameter function.
        /// </summary>
        public async Task<Option<TR>> Apply<T2, T3, TR>(Task<Option<T2>> option2Task,
            Task<Option<T3>> option3Task,
            Func<T1, T2, T3, TR> func)
        {
            ArgumentNullException.ThrowIfNull(option2Task);
            ArgumentNullException.ThrowIfNull(option3Task);
            ArgumentNullException.ThrowIfNull(func);

            var option1 = await option1Task.ConfigureAwait(DefaultConfigureAwait);
            var option2 = await option2Task.ConfigureAwait(DefaultConfigureAwait);
            var option3 = await option3Task.ConfigureAwait(DefaultConfigureAwait);

            return option1.Apply(option2, option3, func);
        }

        /// <summary>
        /// Async variant: Applies four Task Option values to a four-parameter function.
        /// </summary>
        public async Task<Option<TR>> Apply<T2, T3, T4, TR>(Task<Option<T2>> option2Task,
            Task<Option<T3>> option3Task,
            Task<Option<T4>> option4Task,
            Func<T1, T2, T3, T4, TR> func)
        {
            ArgumentNullException.ThrowIfNull(option2Task);
            ArgumentNullException.ThrowIfNull(option3Task);
            ArgumentNullException.ThrowIfNull(option4Task);
            ArgumentNullException.ThrowIfNull(func);

            var option1 = await option1Task.ConfigureAwait(DefaultConfigureAwait);
            var option2 = await option2Task.ConfigureAwait(DefaultConfigureAwait);
            var option3 = await option3Task.ConfigureAwait(DefaultConfigureAwait);
            var option4 = await option4Task.ConfigureAwait(DefaultConfigureAwait);

            return option1.Apply(option2, option3, option4, func);
        }
    }
}


using System;

namespace Roufe;

public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)
    {
        /// <summary>
        /// Applies an Option-wrapped function to an Option-wrapped value.
        /// This is the applicative functor operation for Option.
        /// If either the function or the value is None, returns None.
        /// </summary>
        /// <example>
        /// Option&lt;Func&lt;int, string&gt;&gt; funcOption = Option&lt;Func&lt;int, string&gt;&gt;.From(x => x.ToString());
        /// Option&lt;int&gt; valueOption = Option&lt;int&gt;.From(42);
        /// Option&lt;string&gt; applied = valueOption.Apply(funcOption);
        /// // applied.Value = "42"
        /// </example>
        public Option<TR> Apply<TR>(in Option<Func<T, TR>> funcOption)
        {
            if (funcOption.HasNoValue)
                return Option<TR>.None;

            if (option.HasNoValue)
                return Option<TR>.None;

            return Option<TR>.From(funcOption.GetValueOrThrow()(option.GetValueOrThrow()));
        }
    }

    extension<T, TR>(in Option<Func<T, TR>> funcOption)
    {
        /// <summary>
        /// Applies this Option-wrapped function to an Option-wrapped value.
        /// Convenience overload that reverses the call order.
        /// </summary>
        public Option<TR> Apply(in Option<T> option)
        {
            if (funcOption.HasNoValue)
                return Option<TR>.None;

            if (option.HasNoValue)
                return Option<TR>.None;

            return Option<TR>.From(funcOption.GetValueOrThrow()(option.GetValueOrThrow()));
        }
    }

    /// <summary>
    /// Lifts a regular function into an Option context.
    /// This is the "pure" operation for Option applicative.
    /// </summary>
    public static Option<Func<T, TR>> Pure<T, TR>(Func<T, TR> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return Option<Func<T, TR>>.From(func);
    }

    extension<T1>(in Option<T1> option1)
    {
        /// <summary>
        /// Applies multiple Option values to a multi-parameter function.
        /// All Options must have values for the operation to succeed.
        /// </summary>
        public Option<TR> Apply<T2, TR>(in Option<T2> option2,
            Func<T1, T2, TR> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            if (option1.HasNoValue)
                return Option<TR>.None;

            if (option2.HasNoValue)
                return Option<TR>.None;

            return Option<TR>.From(func(option1.GetValueOrThrow(), option2.GetValueOrThrow()));
        }

        /// <summary>
        /// Applies three Option values to a three-parameter function.
        /// </summary>
        public Option<TR> Apply<T2, T3, TR>(in Option<T2> option2,
            in Option<T3> option3,
            Func<T1, T2, T3, TR> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            if (option1.HasNoValue)
                return Option<TR>.None;

            if (option2.HasNoValue)
                return Option<TR>.None;

            if (option3.HasNoValue)
                return Option<TR>.None;

            return Option<TR>.From(func(
                option1.GetValueOrThrow(),
                option2.GetValueOrThrow(),
                option3.GetValueOrThrow()));
        }

        /// <summary>
        /// Applies four Option values to a four-parameter function.
        /// </summary>
        public Option<TR> Apply<T2, T3, T4, TR>(in Option<T2> option2,
            in Option<T3> option3,
            in Option<T4> option4,
            Func<T1, T2, T3, T4, TR> func)
        {
            ArgumentNullException.ThrowIfNull(func);

            if (option1.HasNoValue)
                return Option<TR>.None;

            if (option2.HasNoValue)
                return Option<TR>.None;

            if (option3.HasNoValue)
                return Option<TR>.None;

            if (option4.HasNoValue)
                return Option<TR>.None;

            return Option<TR>.From(func(
                option1.GetValueOrThrow(),
                option2.GetValueOrThrow(),
                option3.GetValueOrThrow(),
                option4.GetValueOrThrow()));
        }
    }
}


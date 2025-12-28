
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
// ReSharper disable MemberCanBePrivate.Global

namespace Roufe.ValueTasks;

public static partial class AsyncResultExtensionsLeftOperand
{

    public static async ValueTask<Result<IEnumerable<T>, TE>> CombineInOrder<T, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks, Func<IEnumerable<TE>, TE> composerError)
    {
        var results = await CompleteInOrder(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composerError);
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> CombineInOrder<T, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks)
        where TE : ICombine
    {
        var results = await CompleteInOrder(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine();
    }


    public static async ValueTask<Result<IEnumerable<T>, TE>> CombineInOrder<T, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task, Func<IEnumerable<TE>, TE> composerError)
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.CombineInOrder(composerError).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> CombineInOrder<T, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task)
        where TE : ICombine
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.CombineInOrder().ConfigureAwait(DefaultConfigureAwait);
    }

    public static async ValueTask<Result<TK, TE>> CombineInOrder<T, TK, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        IEnumerable<Result<T, TE>> results = await CompleteInOrder(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composer, composerError);
    }

    public static async ValueTask<Result<TK, TE>> CombineInOrder<T, TK, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        IEnumerable<Result<T, TE>> results = await CompleteInOrder(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composer);
    }


    public static async ValueTask<Result<TK, TE>> CombineInOrder<T, TK, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.CombineInOrder(composer, composerError).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async ValueTask<Result<TK, TE>> CombineInOrder<T, TK, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.CombineInOrder(composer).ConfigureAwait(DefaultConfigureAwait);
    }


    public static async ValueTask<T[]> CompleteInOrder<T>(IEnumerable<ValueTask<T>> tasks)
    {
        List<T> results = [];
        foreach (var task in tasks)
        {
            results.Add(await task.ConfigureAwait(DefaultConfigureAwait));
        }
        return results.ToArray();
    }
}

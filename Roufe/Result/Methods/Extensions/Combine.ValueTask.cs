using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable MemberCanBePrivate.Global

namespace Roufe.ValueTasks;

public static partial class AsyncResultExtensionsLeftOperand
{

    public static async ValueTask<Result<IEnumerable<T>, TE>> Combine<T, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks, Func<IEnumerable<TE>, TE> composerError)
    {
        var results = await Task.WhenAll(tasks.Select(x=> x.AsTask())).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composerError);
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> Combine<T, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks)
        where TE : ICombine
    {
        var results = await Task.WhenAll(tasks.Select(x=> x.AsTask())).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine();
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> Combine<T, TE>(this ValueTask<IEnumerable<Result<T, TE>>> task, Func<IEnumerable<TE>, TE> composerError)
    {
        var results = await task.ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composerError);
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> Combine<T, TE>(this ValueTask<IEnumerable<Result<T, TE>>> task)
        where TE : ICombine
    {
        var results = await task.ConfigureAwait(DefaultConfigureAwait);
        return results.Combine();
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> Combine<T, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task, Func<IEnumerable<TE>, TE> composerError)
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine(composerError).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async ValueTask<Result<IEnumerable<T>, TE>> Combine<T, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task)
        where TE : ICombine
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine().ConfigureAwait(DefaultConfigureAwait);
    }

    public static async ValueTask<Result<TK, TE>> Combine<T, TK, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        IEnumerable<Result<T, TE>> results = await Task.WhenAll(tasks.Select(x=> x.AsTask())).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composer, composerError);
    }

    public static async ValueTask<Result<TK, TE>> Combine<T, TK, TE>(this IEnumerable<ValueTask<Result<T, TE>>> tasks, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        IEnumerable<Result<T, TE>> results = await Task.WhenAll(tasks.Select(x=> x.AsTask())).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composer);
    }

    public static async ValueTask<Result<TK, TE>> Combine<T, TK, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine(composer, composerError).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async ValueTask<Result<TK, TE>> Combine<T, TK, TE>(this ValueTask<IEnumerable<ValueTask<Result<T, TE>>>> task, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine(composer).ConfigureAwait(DefaultConfigureAwait);
    }
}

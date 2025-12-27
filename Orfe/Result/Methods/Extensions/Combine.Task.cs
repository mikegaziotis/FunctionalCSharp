using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
// ReSharper disable MemberCanBePrivate.Global

namespace Orfe;

public static partial class AsyncResultExtensionsLeftOperand
{

    public static async Task<Result<IEnumerable<T>, TE>> Combine<T, TE>(this IEnumerable<Task<Result<T, TE>>> tasks, Func<IEnumerable<TE>, TE> composerError)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composerError);
    }

    public static async Task<Result<IEnumerable<T>, TE>> Combine<T, TE>(this IEnumerable<Task<Result<T, TE>>> tasks)
        where TE : ICombine
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine();
    }

    public static async Task<Result<IEnumerable<T>, TE>> Combine<T, TE>(this Task<IEnumerable<Result<T, TE>>> task, Func<IEnumerable<TE>, TE> composerError)
    {
        var results = await task.ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composerError);
    }

    public static async Task<Result<IEnumerable<T>, TE>> Combine<T, TE>(this Task<IEnumerable<Result<T, TE>>> task)
        where TE : ICombine
    {
        var results = await task.ConfigureAwait(DefaultConfigureAwait);
        return results.Combine();
    }


    public static async Task<Result<IEnumerable<T>, TE>> Combine<T, TE>(this Task<IEnumerable<Task<Result<T, TE>>>> task, Func<IEnumerable<TE>, TE> composerError)
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine(composerError).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async Task<Result<IEnumerable<T>, TE>> Combine<T, TE>(this Task<IEnumerable<Task<Result<T, TE>>>> task)
        where TE : ICombine
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine().ConfigureAwait(DefaultConfigureAwait);
    }

    public static async Task<Result<TK, TE>> Combine<T, TK, TE>(this IEnumerable<Task<Result<T, TE>>> tasks, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composer, composerError);
    }

    public static async Task<Result<TK, TE>> Combine<T, TK, TE>(this IEnumerable<Task<Result<T, TE>>> tasks, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(DefaultConfigureAwait);
        return results.Combine(composer);
    }

    public static async Task<Result<TK, TE>> Combine<T, TK, TE>(this Task<IEnumerable<Task<Result<T, TE>>>> task, Func<IEnumerable<T>, TK> composer, Func<IEnumerable<TE>, TE> composerError)
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine(composer, composerError).ConfigureAwait(DefaultConfigureAwait);
    }

    public static async Task<Result<TK, TE>> Combine<T, TK, TE>(this Task<IEnumerable<Task<Result<T, TE>>>> task, Func<IEnumerable<T>, TK> composer)
        where TE : ICombine
    {
        var tasks = await task.ConfigureAwait(DefaultConfigureAwait);
        return await tasks.Combine(composer).ConfigureAwait(DefaultConfigureAwait);
    }

}

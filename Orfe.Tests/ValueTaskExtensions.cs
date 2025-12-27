using System;
using System.Threading.Tasks;
using Orfe.ValueTasks;

namespace Orfe.Tests;

internal static class ValueTaskExtensions
{
    public static ValueTask<T> AsValueTask<T>(this T obj) => obj.AsCompletedValueTask();
    extension(Exception exception)
    {
        public ValueTask AsValueTask() => ValueTask.FromException(exception);
        public ValueTask<T> AsValueTask<T>() => ValueTask.FromException<T>(exception);
    }
}

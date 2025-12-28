using System.Threading.Tasks;

namespace Roufe;

public static class TaskExtensions
{
    extension<T>(T obj)
    {
        public Task<T> AsCompletedTask() => Task.FromResult(obj);
        public ValueTask<T> AsCompletedValueTask() => ValueTask.FromResult(obj);
    }
}

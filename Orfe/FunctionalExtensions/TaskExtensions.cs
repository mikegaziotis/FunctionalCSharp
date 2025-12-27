using System.Threading.Tasks;

namespace Orfe;

public static class TaskExtensions
{
    extension<T>(T obj)
    {
        public Task<T> AsCompletedTask() => Task.FromResult(obj);
        public ValueTask<T> AsCompletedValueTask() => ValueTask.FromResult(obj);
    }
}

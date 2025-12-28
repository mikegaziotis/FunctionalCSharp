using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class PipeExtensionsTests
{
    [Fact]
    public void Pipe_ReturnsResult()
    {
        var input = "hello";
        var result = input.Pipe(s => s.Length);
        Assert.Equal(5, result);
    }

    [Fact]
    public async Task PipeAsync_ReturnsResultAsync()
    {
        var input = 3;
        var result = await input.PipeAsync(n => Task.FromResult(n * 2));
        Assert.Equal(6, result);
    }

}

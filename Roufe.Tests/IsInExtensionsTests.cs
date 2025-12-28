using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class IsInExtensionsTests
{

    [Theory]
    [InlineData(3, true)]
    [InlineData(6, false)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(5, true)]
    [InlineData(1, true)]
    public void IsIn_WithValueInCollection_Tests(int value, bool result)
    {
        IEnumerable<int> collection = [1, 2, 3, 4, 5];
        Assert.Equal(value.IsIn(collection), result);
    }

    [Theory]
    [InlineData(3, true)]
    [InlineData(6, false)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(5, true)]
    [InlineData(1, true)]
    public void IsIn_WithValueInParamsCollection_Tests(int value, bool result)
    {
        Assert.Equal(value.IsIn(1,2,3,4,5), result);
    }

    [Fact]
    public async Task IsIn_WithNullCollection_ThrowsArgumentNullException()
    {
        IEnumerable<int> collection = null!;
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = 3.IsIn(collection);
            await Task.CompletedTask;
        });
    }
}

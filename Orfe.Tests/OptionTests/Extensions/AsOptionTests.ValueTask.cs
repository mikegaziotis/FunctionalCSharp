using System.Threading.Tasks;
using Orfe.ValueTasks;
using Xunit;

namespace Orfe.Tests.OptionTests.Extensions;

public class AsOptionTests_ValueTask : TestBase
{
    [Fact]
    public async Task AsOption_ValueTask_Struct_Option_conversion_equality_none()
    {
        double? none = null;
        var optionNone = await none.AsValueTask().AsOption();
        Assert.Equal(none.HasValue, optionNone.HasValue);
    }

    [Fact]
    public async Task AsOption_ValueTask_Struct_Option_conversion_equality_some()
    {
        double? some = 123;
        var someOption = await some.AsValueTask().AsOption();
        Assert.Equal(some.HasValue, someOption.HasValue);
        Assert.Equal(some, someOption.Value);
    }

    [Fact]
    public async Task AsOption_ValueTask_Class_Option_conversion_none()
    {
        var optionT = await Option<T>.None.AsValueTask();
        Assert.False(optionT.HasValue);
    }

    [Fact]
    public async Task AsOption_ValueTask_Class_Option_conversion_some()
    {
        var optionT = (await T.Value.AsValueTask()).AsOption();
        Assert.True(optionT.HasValue);
        Assert.Equal(T.Value, optionT.Value);
    }
}

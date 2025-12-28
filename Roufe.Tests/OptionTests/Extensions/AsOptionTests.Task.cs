using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests.OptionTests.Extensions;

public class AsOptionTests_Task : TestBase
{
    [Fact]
    public async Task AsOption_Task_Struct_Option_conversion_equality_none()
    {
        double? none = null;
        var optionNone = await none.AsTask().AsOption();
        Assert.Equal(none.HasValue, optionNone.HasValue);
    }

    [Fact]
    public async Task AsOption_Task_Struct_Option_conversion_equality_some()
    {
        double? some = 123;
        var someOption = await some.AsTask().AsOption();
        Assert.Equal(some.HasValue, someOption.HasValue);
        Assert.Equal(some, someOption.Value);
    }

    [Fact]
    public async Task AsOption_Task_Class_Option_conversion_none()
    {
        var optionT = await Option<T>.None.AsTask();
        Assert.False(optionT.HasValue);
    }

    [Fact]
    public async Task AsOption_Task_Class_Option_conversion_some()
    {
        var optionT = await T.Value.AsOption().AsTask();
        Assert.True(optionT.HasValue);
        Assert.Equal(T.Value, optionT.Value);
    }
}

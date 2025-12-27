using Xunit;

namespace Orfe.Tests.OptionTests.Extensions;

public class AsOptionTests : TestBase
{
    [Fact]
    public void AsOption_Struct_Option_conversion_equality_none()
    {
        double? none = null;
        var optionNone = none.AsOption();

        Assert.Equal(optionNone.HasValue, none.HasValue);
    }

    [Fact]
    public void AsOption_Struct_Option_conversion_equality_some()
    {
        double? some = 123;
        var someOption = some.AsOption();

        Assert.Equal(someOption.HasValue, some.HasValue);
        Assert.Equal(someOption.Value,some);
    }

    [Fact]
    public void AsOption_Class_Option_conversion_none()
    {
        Option<T> optionT = null;

        Assert.False(optionT.HasValue);
    }

    [Fact]
    public void AsOption_Class_Option_conversion_some()
    {
        var optionT = T.Value.AsOption();

        Assert.True(optionT.HasValue);
        Assert.Equal(optionT.Value,T.Value);
    }
}

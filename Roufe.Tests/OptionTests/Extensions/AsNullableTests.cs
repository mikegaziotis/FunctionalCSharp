using Xunit;

namespace Roufe.Tests.OptionTests.Extensions;

public class AsNullableTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(5.0)]
    public void Struct_nullable_conversion_equality(double? value)
    {
        Option<double> option = value ?? 0;
        var nullable = option.AsNullable();

        Assert.Equal(option.HasValue, nullable.HasValue);
        Assert.Equal(nullable, value);

        if (value.HasValue)
        {
            Assert.Equal(value.Value, option.Value);
        }
    }
}

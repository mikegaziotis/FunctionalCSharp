using Xunit;

namespace Roufe.Tests;

public class UnitTests
{
    [Fact]
    public void Unit_Equals_OtherUnit_ReturnsTrue()
    {
        var unit1 = Unit.Value;
        var unit2 = Unit.Value;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
    }

    [Fact]
    public void Unit_NotEquals_OtherUnit_ReturnsFalse()
    {
        var unit1 = Unit.Value;
        var unit2 = Unit.Value;

        Assert.False(unit1!=unit2);
    }
}

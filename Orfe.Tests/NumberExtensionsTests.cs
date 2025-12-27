using Xunit;

namespace Orfe.Tests;

public class NumberExtensionsTests
{
    [Theory]
    [InlineData(3,5, 10, NumberExtensions.InclusionType.InclusiveBothEnds, false)]
    [InlineData(11,5, 10, NumberExtensions.InclusionType.InclusiveBothEnds, false)]
    [InlineData(6,5, 10, NumberExtensions.InclusionType.InclusiveBothEnds, true)]
    [InlineData(6,10, 5, NumberExtensions.InclusionType.InclusiveBothEnds, true)]
    [InlineData(11,10, 5, NumberExtensions.InclusionType.InclusiveBothEnds, false)]
    [InlineData(5,5, 10, NumberExtensions.InclusionType.InclusiveBothEnds, true)]
    [InlineData(5,5, 10, NumberExtensions.InclusionType.InclusiveLowerExclusiveUpper, true)]
    [InlineData(5,5, 10, NumberExtensions.InclusionType.ExclusiveBothEnds, false)]
    [InlineData(5,5, 10, NumberExtensions.InclusionType.ExclusiveLoweInclusiveUpper, false)]
    [InlineData(10,5, 10, NumberExtensions.InclusionType.ExclusiveBothEnds, false)]
    [InlineData(10,5, 10, NumberExtensions.InclusionType.InclusiveLowerExclusiveUpper, false)]
    [InlineData(10,5, 10, NumberExtensions.InclusionType.InclusiveBothEnds, true)]
    [InlineData(10,5, 10, NumberExtensions.InclusionType.ExclusiveLoweInclusiveUpper, true)]
    public void IsBetweenTest_Ints(int value, int lower, int upper, NumberExtensions.InclusionType inclusionType, bool expected)
    {
        Assert.Equal(value.IsBetween(lower, upper, inclusionType), expected);
    }

    // GreaterThan tests
    [Theory]
    [InlineData(5,3,true)]
    [InlineData(3,5,false)]
    [InlineData(5,5,false)]
    public void GreaterThan_Ints(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThan(other));
    }

    [Theory]
    [InlineData(5.5, 3.2, true)]
    [InlineData(3.2, 5.5, false)]
    [InlineData(5.0, 5.0, false)]
    public void GreaterThan_Doubles(double value, double other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThan(other));
    }

    [Theory]
    [InlineData(5.5f, 3.2f, true)]
    [InlineData(3.2f, 5.5f, false)]
    [InlineData(5.0f, 5.0f, false)]
    public void GreaterThan_Floats(float value, float other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThan(other));
    }

    [Theory]
    [InlineData(5, 3.0, true)]
    [InlineData(3, 5.0, false)]
    [InlineData(5, 5.0, false)]
    public void GreaterThan_Mixed_Int_Double(int value, double other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThan(other));
    }

    // GreaterThanOrEqualTo tests
    [Theory]
    [InlineData(5,3,true)]
    [InlineData(3,5,false)]
    [InlineData(5,5,true)]
    public void GreaterThanOrEqualTo_Ints(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThanOrEqualTo(other));
    }

    [Theory]
    [InlineData(5.5, 3.2, true)]
    [InlineData(3.2, 5.5, false)]
    [InlineData(5.0, 5.0, true)]
    public void GreaterThanOrEqualTo_Doubles(double value, double other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThanOrEqualTo(other));
    }

    [Theory]
    [InlineData(5.5f, 3.2f, true)]
    [InlineData(3.2f, 5.5f, false)]
    [InlineData(5.0f, 5.0f, true)]
    public void GreaterThanOrEqualTo_Floats(float value, float other, bool expected)
    {
        Assert.Equal(expected, value.GreaterThanOrEqualTo(other));
    }

    // LessThanOrEqualTo tests
    [Theory]
    [InlineData(5,3,false)]
    [InlineData(3,5,true)]
    [InlineData(5,5,true)]
    public void LessThanOrEqualTo_Ints(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.LessThanOrEqualTo(other));
    }

    [Theory]
    [InlineData(5.5, 3.2, false)]
    [InlineData(3.2, 5.5, true)]
    [InlineData(5.0, 5.0, true)]
    public void LessThanOrEqualTo_Doubles(double value, double other, bool expected)
    {
        Assert.Equal(expected, value.LessThanOrEqualTo(other));
    }

    [Theory]
    [InlineData(5.5f, 3.2f, false)]
    [InlineData(3.2f, 5.5f, true)]
    [InlineData(5.0f, 5.0f, true)]
    public void LessThanOrEqualTo_Floats(float value, float other, bool expected)
    {
        Assert.Equal(expected, value.LessThanOrEqualTo(other));
    }

    // Mixed-type sanity checks
    [Fact]
    public void MixedTypeComparisons()
    {
        int i = 5;
        double d = 5.0;
        float f = 4.9f;

        Assert.True(i.GreaterThan(f));         // int vs float
        Assert.True(d.GreaterThanOrEqualTo(i)); // double vs int (equal)
        Assert.True(f.LessThanOrEqualTo(d));   // float vs double
    }

    // Mixed-type IsBetween tests
    [Fact]
    public void IsBetween_Mixed_IntDoubleFloat_Inclusive()
    {
        // int value, double lower, float upper
        Assert.True(5.IsBetween(4.5, 6f)); // 4.5 <= 5 <= 6.0
    }

    [Fact]
    public void IsBetween_Mixed_ReversedBounds_IntDoubleFloat()
    {
        // bounds provided in reverse order (float, int)
        Assert.True(5.IsBetween(6.0f, 4)); // should swap and evaluate true
    }

    [Fact]
    public void IsBetween_Mixed_ExclusiveBothEnds_FalseWhenEqualToBounds()
    {
        // exclusive both ends should return false when value equals a bound
        Assert.False(5.IsBetween(5.0, 10f, NumberExtensions.InclusionType.ExclusiveBothEnds));
        Assert.False(10.IsBetween(5, 10.0, NumberExtensions.InclusionType.ExclusiveBothEnds));
    }

    [Fact]
    public void IsBetween_WithNaN_ReturnsFalse_ForAnyNaNOperand()
    {
        Assert.False(double.NaN.IsBetween(0, 1));
        Assert.False(5.IsBetween(double.NaN, 6));
        Assert.False(5.IsBetween(4, float.NaN));
    }

    [Fact]
    public void IsBetween_Mixed_NegativeValues()
    {
        // negative ranges with mixed types
        Assert.True((-2).IsBetween(-3.5, -1f));
        Assert.True((-2.5f).IsBetween(-5, -2.0)); // float value, int lower, double upper
    }
}

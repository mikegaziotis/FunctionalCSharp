using Xunit;

namespace Orfe.Tests.OptionTests.Extensions;

public class BindOptionalTests
{
    [Fact]
    public void BindOptional_with_value_returns_value()
    {
        Option<int> option = 42;
        var result = option.BindOptional(i => Result.Success(i.ToString()));

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.HasValue);
        Assert.Equal("42", result.Value.Value);
    }

    [Fact]
    public void BindOptional_missing_value_returns_missing()
    {
        var option = Option<int>.None;
        var result = option.BindOptional(i => Result.Success(i.ToString()));
        Assert.True(result.IsSuccess);
        Assert.False(result.Value.HasValue);
    }

    [Fact]
    public void BindOptional_with_error_returns_error()
    {
        Option<int> option = 42;
        var result = option.BindOptional(i => Result.Failure<string>("Something went wrong"));
        Assert.False(result.IsSuccess);
        Assert.Equal("Something went wrong", result.Error);
    }

    [Fact]
    public void BindOptional_E_with_value_returns_value()
    {
        Option<int> option = 42;
        var result = option.BindOptional(i => Result.Success<string, string>(i.ToString()));
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.HasValue);
        Assert.Equal("42", result.Value.Value);
    }

    [Fact]
    public void BindOptional_E_missing_value_returns_missing()
    {
        var option = Option<int>.None;
        var result = option.BindOptional(i => Result.Success<string, string>(i.ToString()));
        Assert.True(result.IsSuccess);
        Assert.False(result.Value.HasValue);
    }

    [Fact]
    public void BindOptional_E_with_error_returns_error()
    {
        Option<int> option = 42;
        var result = option.BindOptional(i => Result.Failure<string, string>("Something went wrong"));
        Assert.False(result.IsSuccess);
        Assert.Equal("Something went wrong", result.Error);
    }
}

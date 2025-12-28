using System;
using Xunit;

namespace Roufe.Tests.OptionTests.Extensions;

public class BindTests : TestBase
{
    [Fact]
    public void Bind_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> option = null;
        var option2 = option.Bind(_ => Option.From(K.Value));

        Assert.False(option2.HasValue);
    }

    [Fact]
    public void Bind_returns_no_value_if_selector_returns_none()
    {
        Option<T> option = T.Value;

        var option2 = option.Bind(_ => Option<K>.None);

        Assert.False(option2.HasValue);
    }

    [Fact]
    public void Bind_returns_value_if_selector_returns_value()
    {
        Option<T> option = T.Value;

        var option2 = option.Bind(_ => Option.From(T.Value));

        Assert.True(option2.HasValue);
        Assert.Equal(T.Value,option2.Value);
    }

    [Fact]
    public void Bind_provides_context_to_selector()
    {
        const int context = 5;
        Option<T> option = T.Value;

        var option2 = option.Bind(
            (value, ctx) =>
            {
                Assert.Equal(context,ctx);
                return Option.From(value);
            },
            context
        );

        Assert.True(option2.HasValue);
    }

    [Fact]
    public void Bind_with_context_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> option = null;

        var option2 = option.Bind(
            (value, _) => Option.From(value),
            context: 5
        );

        Assert.False(option2.HasValue);
    }

    [Fact]
    public void Bind_with_context_returns_no_value_if_selector_returns_null()
    {
        Option<T> option = T.Value;

        var option2 = option.Bind(
            (value, _) =>
            {
                Assert.Equal(value, T.Value);
                return Option<K>.None;
            },
            context: 5
        );

        Assert.False(option2.HasValue);
    }

    [Fact]
    public void Bind_with_context_returns_value_if_selector_returns_value()
    {
        Option<T> option = T.Value;

        var option2 = option.Bind((value, _) =>
            {
                Assert.Equal(value, T.Value);
                return Option.From(value);
            },
            5
        );

        Assert.True(option2.HasValue);
        Assert.Equal(option2.Value,T.Value);
    }
}

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Roufe.Tests.OptionTests.Extensions;

public class BindTests_Task_Left : TestBase
{
    [Fact]
    public async Task Bind_Task_Left_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> Option = null;

        var Option2 = await Option.AsTask().Bind(ExpectAndReturnOption<T>(null, T.Value2));

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_Left_returns_no_value_if_selector_returns_null()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(ExpectAndReturn(T.Value, Option<T>.None));

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_Left_returns_value_if_selector_returns_value()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(ExpectAndReturnOption<T>(T.Value, T.Value2));

        Option2.HasValue.Should().BeTrue();
        Option2.Value.Should().Be(T.Value2);
    }

    [Fact]
    public async Task Bind_Task_Left_provides_context_to_selector()
    {
        Option<T> Option = T.Value;
        var context = 5;

        var Option2 = await Option.AsTask().Bind(
            (value, ctx) =>
            {
                ctx.Should().Be(context);
                return Option.From(value);
            },
            context
        );

        Option2.HasValue.Should().BeTrue();
    }

    [Fact]
    public async Task Bind_Task_Left_with_context_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> Option = null;

        var Option2 = await Option.AsTask().Bind(
            (value, _) => ExpectAndReturnOption(null, T.Value)(value),
            context: 5
        );

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_Left_with_context_returns_no_value_if_selector_returns_null()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(
            (value, _) => ExpectAndReturn(T.Value, Option<T>.None)(value),
            context: 5
        );

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_Left_with_context_returns_value_if_selector_returns_value()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(
            (value, _) => ExpectAndReturnOption<T>(T.Value, T.Value)(value),
            5
        );

        Option2.HasValue.Should().BeTrue();
        Option2.Value.Should().Be(T.Value);
    }
}

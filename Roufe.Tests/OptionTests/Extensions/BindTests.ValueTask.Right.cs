using System.Threading.Tasks;
using CSharpFunctionalExtensions.ValueTasks;
using FluentAssertions;
using Xunit;

namespace Roufe.Tests.OptionTests.Extensions;

public class BindTests_ValueTask_Right : TestBase
{
    [Fact]
    public async Task Bind_ValueTask_Right_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> Option = null;

        var Option2 = await Option.Bind(ExpectAndReturnOption_ValueTask<T>(null, T.Value2));

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_ValueTask_Right_returns_no_value_if_selector_returns_null()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.Bind(ExpectAndReturn_ValueTask(T.Value, Option<T>.None));

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_ValueTask_Right_returns_value_if_selector_returns_value()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.Bind(ExpectAndReturnOption_ValueTask<T>(T.Value, T.Value2));

        Option2.HasValue.Should().BeTrue();
        Option2.Value.Should().Be(T.Value2);
    }

    [Fact]
    public async Task Bind_ValueTask_Right_provides_context_to_selector()
    {
        Option<T> Option = T.Value;
        var context = 5;

        var Option2 = await Option.Bind(
            (value, ctx) =>
            {
                ctx.Should().Be(context);
                return Option.From(value).AsTask();
            },
            context
        );

        Option2.HasValue.Should().BeTrue();
    }

    [Fact]
    public async Task Bind_ValueTask_Right_with_context_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> Option = null;

        var Option2 = await Option.Bind(
            (value, _) => ExpectAndReturnOption_ValueTask(null, T.Value)(value),
            context: 5
        );

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_ValueTask_Right_with_context_returns_no_value_if_selector_returns_null()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.Bind(
            (value, _) => ExpectAndReturn_ValueTask(T.Value, Option<T>.None)(value),
            context: 5
        );

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_ValueTask_Right_with_context_returns_value_if_selector_returns_value()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.Bind(
            (value, _) => ExpectAndReturnOption_ValueTask<T>(T.Value, T.Value)(value),
            5
        );

        Option2.HasValue.Should().BeTrue();
        Option2.Value.Should().Be(T.Value);
    }
}

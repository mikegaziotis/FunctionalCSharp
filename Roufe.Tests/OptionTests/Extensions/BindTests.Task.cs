using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Roufe.Tests.OptionTests.Extensions;

public class BindTests_Task : TestBase
{
    [Fact]
    public async Task Bind_Task_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> Option = null;

        var Option2 = await Option.AsTask().Bind(ExpectAndReturnOption_Task(null, T.Value));

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_returns_no_value_if_selector_returns_null()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(ExpectAndReturn_Task(T.Value, Option<T>.None));

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_returns_value_if_selector_returns_value()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(ExpectAndReturnOption_Task<T>(T.Value, T.Value));

        Option2.HasValue.Should().BeTrue();
        Option2.Value.Should().Be(T.Value);
    }

    [Fact]
    public async Task Bind_Task_provides_context_to_selector()
    {
        Option<T> Option = T.Value;
        var context = 5;

        var Option2 = await Option.AsTask().Bind(
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
    public async Task Bind_Task_with_context_returns_no_value_if_initial_Option_is_null()
    {
        Option<T> Option = null;

        var Option2 = await Option.AsTask().Bind(
            (value, _) => ExpectAndReturnOption_Task(null, T.Value)(value),
            context: 5
        );

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_with_context_returns_no_value_if_selector_returns_null()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(
            (value, _) => ExpectAndReturn_Task(T.Value, Option<T>.None)(value),
            context: 5
        );

        Option2.HasValue.Should().BeFalse();
    }

    [Fact]
    public async Task Bind_Task_with_context_returns_value_if_selector_returns_value()
    {
        Option<T> Option = T.Value;

        var Option2 = await Option.AsTask().Bind(
            (value, _) => ExpectAndReturnOption_Task<T>(T.Value, T.Value)(value),
            5
        );

        Option2.HasValue.Should().BeTrue();
        Option2.Value.Should().Be(T.Value);
    }
}

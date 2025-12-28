using System.Threading.Tasks;
using Roufe.ValueTasks;
using FluentAssertions;
using Roufe;
using Xunit;

namespace Roufe.Tests.OptionTests.Extensions
{
    public class BindTests_ValueTask : TestBase
    {
        [Fact]
        public async Task Bind_ValueTask_returns_no_value_if_initial_Option_is_null()
        {
            Option<T> Option = null;

            var Option2 = await Option.AsValueTask().Bind(ExpectAndReturnOption_ValueTask(null, T.Value));

            Option2.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task Bind_ValueTask_returns_no_value_if_selector_returns_null()
        {
            Option<T> Option = T.Value;

            var Option2 = await Option.AsValueTask().Bind(ExpectAndReturn_ValueTask(T.Value, Option<T>.None));

            Option2.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task Bind_ValueTask_returns_value_if_selector_returns_value()
        {
            Option<T> Option = T.Value;

            var Option2 = await Option.AsValueTask().Bind(ExpectAndReturnOption_ValueTask<T>(T.Value, T.Value));

            Option2.HasValue.Should().BeTrue();
            Option2.Value.Should().Be(T.Value);
        }

        [Fact]
        public async Task Bind_ValueTask_provides_context_to_selector()
        {
            Option<T> Option = T.Value;
            var context = 5;

            var Option2 = await Option.AsValueTask().Bind(
                (value, ctx) =>
                {
                    ctx.Should().Be(context);
                    return Option.From(value).AsValueTask();
                },
                context
            );

            Option2.HasValue.Should().BeTrue();
        }

        [Fact]
        public async Task Bind_ValueTask_with_context_returns_no_value_if_initial_Option_is_null()
        {
            Option<T> Option = null;

            var Option2 = await Option.AsValueTask().Bind(
                (value, _) => ExpectAndReturnOption_ValueTask(null, T.Value)(value),
                context: 5
            );

            Option2.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task Bind_ValueTask_with_context_returns_no_value_if_selector_returns_null()
        {
            Option<T> Option = T.Value;

            var Option2 = await Option.AsValueTask().Bind(
                (value, _) => ExpectAndReturn_ValueTask(T.Value, Option<T>.None)(value),
                context: 5
            );

            Option2.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task Bind_ValueTask_with_context_returns_value_if_selector_returns_value()
        {
            Option<T> Option = T.Value;

            var Option2 = await Option.AsValueTask().Bind(
                (value, _) => ExpectAndReturnOption_ValueTask<T>(T.Value, T.Value)(value),
                5
            );

            Option2.HasValue.Should().BeTrue();
            Option2.Value.Should().Be(T.Value);
        }
    }
}

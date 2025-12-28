using System;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class OptionApplyTests
{
    #region Basic Apply Tests

    [Fact]
    public void Apply_BothSome_AppliesFunctionToValue()
    {
        // Arrange
        var funcOption = Option<Func<int, string>>.From(x => x.ToString());
        var valueOption = Option<int>.From(42);

        // Act
        var applied = valueOption.Apply(funcOption);

        // Assert
        Assert.True(applied.HasValue);
        Assert.Equal("42", applied.GetValueOrThrow());
    }

    [Fact]
    public void Apply_FunctionNone_ReturnsNone()
    {
        // Arrange
        var funcOption = Option<Func<int, string>>.None;
        var valueOption = Option<int>.From(42);

        // Act
        var applied = valueOption.Apply(funcOption);

        // Assert
        Assert.True(applied.HasNoValue);
    }

    [Fact]
    public void Apply_ValueNone_ReturnsNone()
    {
        // Arrange
        var funcOption = Option<Func<int, string>>.From(x => x.ToString());
        var valueOption = Option<int>.None;

        // Act
        var applied = valueOption.Apply(funcOption);

        // Assert
        Assert.True(applied.HasNoValue);
    }

    [Fact]
    public void Apply_BothNone_ReturnsNone()
    {
        // Arrange
        var funcOption = Option<Func<int, string>>.None;
        var valueOption = Option<int>.None;

        // Act
        var applied = valueOption.Apply(funcOption);

        // Assert
        Assert.True(applied.HasNoValue);
    }

    [Fact]
    public void Apply_ReversedCall_WorksCorrectly()
    {
        // Arrange
        var funcOption = Option<Func<int, string>>.From(x => $"Value: {x}");
        var valueOption = Option<int>.From(42);

        // Act
        var applied = funcOption.Apply(valueOption);

        // Assert
        Assert.True(applied.HasValue);
        Assert.Equal("Value: 42", applied.GetValueOrThrow());
    }

    #endregion

    #region Pure Tests

    [Fact]
    public void Pure_CreatesSomeOptionWithFunction()
    {
        // Arrange
        Func<int, string> func = x => x.ToString();

        // Act
        var option = OptionExtensions.Pure<int, string>(func);

        // Assert
        Assert.True(option.HasValue);
        Assert.NotNull(option.GetValueOrThrow());
        Assert.Equal("42", option.GetValueOrThrow()(42));
    }

    #endregion

    #region Multi-Parameter Apply Tests

    [Fact]
    public void Apply_TwoParameters_BothSome_CombinesValues()
    {
        // Arrange
        var option1 = Option<int>.From(10);
        var option2 = Option<int>.From(32);

        // Act
        var combined = option1.Apply(option2, (a, b) => a + b);

        // Assert
        Assert.True(combined.HasValue);
        Assert.Equal(42, combined.GetValueOrThrow());
    }

    [Fact]
    public void Apply_TwoParameters_FirstNone_ReturnsNone()
    {
        // Arrange
        var option1 = Option<int>.None;
        var option2 = Option<int>.From(32);

        // Act
        var combined = option1.Apply(option2, (a, b) => a + b);

        // Assert
        Assert.True(combined.HasNoValue);
    }

    [Fact]
    public void Apply_TwoParameters_SecondNone_ReturnsNone()
    {
        // Arrange
        var option1 = Option<int>.From(10);
        var option2 = Option<int>.None;

        // Act
        var combined = option1.Apply(option2, (a, b) => a + b);

        // Assert
        Assert.True(combined.HasNoValue);
    }

    [Fact]
    public void Apply_ThreeParameters_AllSome_CombinesValues()
    {
        // Arrange
        var option1 = Option<int>.From(10);
        var option2 = Option<int>.From(20);
        var option3 = Option<int>.From(12);

        // Act
        var combined = option1.Apply(option2, option3, (a, b, c) => a + b + c);

        // Assert
        Assert.True(combined.HasValue);
        Assert.Equal(42, combined.GetValueOrThrow());
    }

    [Fact]
    public void Apply_FourParameters_AllSome_CombinesValues()
    {
        // Arrange
        var option1 = Option<int>.From(10);
        var option2 = Option<int>.From(10);
        var option3 = Option<int>.From(10);
        var option4 = Option<int>.From(12);

        // Act
        var combined = option1.Apply(option2, option3, option4, (a, b, c, d) => a + b + c + d);

        // Assert
        Assert.True(combined.HasValue);
        Assert.Equal(42, combined.GetValueOrThrow());
    }

    #endregion

    #region Task Apply Tests

    [Fact]
    public async Task Apply_TaskOptions_BothSome_AppliesFunction()
    {
        // Arrange
        var funcTask = Task.FromResult(Option<Func<int, string>>.From(x => x.ToString()));
        var valueTask = Task.FromResult(Option<int>.From(42));

        // Act
        var applied = await valueTask.Apply(funcTask);

        // Assert
        Assert.True(applied.HasValue);
        Assert.Equal("42", applied.GetValueOrThrow());
    }

    [Fact]
    public async Task Apply_TaskFunc_SyncValue_Works()
    {
        // Arrange
        var funcTask = Task.FromResult(Option<Func<int, string>>.From(x => x.ToString()));
        var value = Option<int>.From(42);

        // Act
        var applied = await value.Apply(funcTask);

        // Assert
        Assert.True(applied.HasValue);
        Assert.Equal("42", applied.GetValueOrThrow());
    }

    [Fact]
    public async Task Apply_TwoTaskParameters_AllSome_CombinesValues()
    {
        // Arrange
        var task1 = Task.FromResult(Option<int>.From(10));
        var task2 = Task.FromResult(Option<int>.From(32));

        // Act
        var combined = await task1.Apply(task2, (a, b) => a + b);

        // Assert
        Assert.True(combined.HasValue);
        Assert.Equal(42, combined.GetValueOrThrow());
    }

    #endregion

    #region ValueTask Apply Tests

    [Fact]
    public async Task Apply_ValueTaskOptions_BothSome_AppliesFunction()
    {
        // Arrange
        var funcTask = ValueTask.FromResult(Option<Func<int, string>>.From(x => x.ToString()));
        var valueTask = ValueTask.FromResult(Option<int>.From(42));

        // Act
        var applied = await valueTask.Apply(funcTask);

        // Assert
        Assert.True(applied.HasValue);
        Assert.Equal("42", applied.GetValueOrThrow());
    }

    [Fact]
    public async Task Apply_TwoValueTaskParameters_AllSome_CombinesValues()
    {
        // Arrange
        var task1 = ValueTask.FromResult(Option<int>.From(10));
        var task2 = ValueTask.FromResult(Option<int>.From(32));

        // Act
        var combined = await task1.Apply(task2, (a, b) => a + b);

        // Assert
        Assert.True(combined.HasValue);
        Assert.Equal(42, combined.GetValueOrThrow());
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_OptionalFieldCombination()
    {
        // Arrange - Simulating optional form fields
        var firstName = Option<string>.From("John");
        var lastName = Option<string>.From("Doe");

        // Act - Combine into full name
        var fullName = firstName.Apply(lastName, (first, last) => $"{first} {last}");

        // Assert
        Assert.True(fullName.HasValue);
        Assert.Equal("John Doe", fullName.GetValueOrThrow());
    }

    [Fact]
    public void Scenario_OptionalFieldCombination_OneMissing()
    {
        // Arrange - One field is missing
        var firstName = Option<string>.From("John");
        var lastName = Option<string>.None;

        // Act
        var fullName = firstName.Apply(lastName, (first, last) => $"{first} {last}");

        // Assert
        Assert.True(fullName.HasNoValue);
    }

    [Fact]
    public void Scenario_IndependentLookups_AllPresent()
    {
        // Arrange - Three independent dictionary lookups
        var dict = new System.Collections.Generic.Dictionary<string, string>
        {
            ["street"] = "123 Main St",
            ["city"] = "Springfield",
            ["zip"] = "12345"
        };

        var street = TryGetValue(dict, "street");
        var city = TryGetValue(dict, "city");
        var zip = TryGetValue(dict, "zip");

        // Act - Combine into address
        var address = street.Apply(city, zip,
            (s, c, z) => $"{s}, {c} {z}");

        // Assert
        Assert.True(address.HasValue);
        Assert.Equal("123 Main St, Springfield 12345", address.GetValueOrThrow());
    }

    [Fact]
    public void Scenario_ConfigurationValidation()
    {
        // Arrange - Multiple optional configuration values
        var host = Option<string>.From("localhost");
        var port = Option<int>.From(8080);
        var path = Option<string>.From("/api");

        // Act - Combine into URL
        var url = host.Apply(port, path,
            (h, p, pa) => $"http://{h}:{p}{pa}");

        // Assert
        Assert.True(url.HasValue);
        Assert.Equal("http://localhost:8080/api", url.GetValueOrThrow());
    }

    [Fact]
    public async Task Scenario_ParallelOptionalFetch_Async()
    {
        // Arrange - Simulate async optional data fetches
        var emailTask = FetchOptionalEmailAsync(1);
        var phoneTask = FetchOptionalPhoneAsync(1);

        // Act - Combine both if present
        var contactInfo = await emailTask.Apply(phoneTask,
            (email, phone) => new { Email = email, Phone = phone });

        // Assert
        Assert.True(contactInfo.HasValue);
        Assert.Equal("user@example.com", contactInfo.GetValueOrThrow().Email);
        Assert.Equal("555-1234", contactInfo.GetValueOrThrow().Phone);
    }

    [Fact]
    public void Scenario_ApplyWithPure_LiftFunctionIntoOption()
    {
        // Arrange
        var value = Option<int>.From(42);
        var func = OptionExtensions.Pure<int, string>(x => $"The answer is {x}");

        // Act
        var result = value.Apply(func);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal("The answer is 42", result.GetValueOrThrow());
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void Apply_MultiParam_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        var option1 = Option<int>.From(10);
        var option2 = Option<int>.From(32);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            option1.Apply(option2, (Func<int, int, int>)null!));
    }

    [Fact]
    public void Pure_NullFunction_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            OptionExtensions.Pure<int, string>(null!));
    }

    #endregion

    #region Helper Methods

    private Option<string> TryGetValue(System.Collections.Generic.Dictionary<string, string> dict, string key)
    {
        return dict.TryGetValue(key, out var value)
            ? Option<string>.From(value)
            : Option<string>.None;
    }

    private Task<Option<string>> FetchOptionalEmailAsync(int userId)
    {
        return Task.FromResult(Option<string>.From("user@example.com"));
    }

    private Task<Option<string>> FetchOptionalPhoneAsync(int userId)
    {
        return Task.FromResult(Option<string>.From("555-1234"));
    }

    #endregion
}


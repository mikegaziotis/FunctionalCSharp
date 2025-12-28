using System;
using Xunit;

namespace Roufe.Tests;

public class FunctionExtensionsTests
{
    #region Composition Tests

    [Fact]
    public void Compose_ShouldApplyFunctionsInReverseOrder()
    {
        // Arrange
        Func<int, int> addOne = x => x + 1;
        Func<int, int> multiplyByTwo = x => x * 2;

        // Act
        var composed = addOne.Compose(multiplyByTwo);
        var result = composed(5);

        // Assert
        Assert.Equal(11, result); // (5 * 2) + 1
    }

    [Fact]
    public void AndThen_ShouldApplyFunctionsInOrder()
    {
        // Arrange
        Func<int, int> addOne = x => x + 1;
        Func<int, int> multiplyByTwo = x => x * 2;

        // Act
        var chained = addOne.AndThen(multiplyByTwo);
        var result = chained(5);

        // Assert
        Assert.Equal(12, result); // (5 + 1) * 2
    }

    [Fact]
    public void Compose_AndThen_ChainedOperations()
    {
        // Arrange
        Func<int, int> addOne = x => x + 1;
        Func<int, int> multiplyByTwo = x => x * 2;
        Func<int, int> square = x => x * x;

        // Act
        var result = addOne
            .AndThen(multiplyByTwo)
            .AndThen(square)
            .Invoke()(5); // (((5 + 1) * 2) ^ 2) = 144

        // Assert
        Assert.Equal(144, result);
    }

    #endregion

    #region Currying Tests

    [Fact]
    public void Curry_TwoParameters_ShouldWork()
    {
        // Arrange
        Func<int, int, int> add = (a, b) => a + b;

        // Act
        var curriedAdd = add.Curry();
        var add5 = curriedAdd(5);
        var result = add5(3);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Curry_ThreeParameters_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int> addThree = (a, b, c) => a + b + c;

        // Act
        var curriedAdd = addThree.Curry();
        var result = curriedAdd(1)(2)(3);

        // Assert
        Assert.Equal(6, result);
    }

    [Fact]
    public void Curry_FourParameters_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int, int> addFour = (a, b, c, d) => a + b + c + d;

        // Act
        var curriedAdd = addFour.Curry();
        var result = curriedAdd(1)(2)(3)(4);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void Uncurry_TwoParameters_ShouldWork()
    {
        // Arrange
        Func<int, Func<int, int>> curriedAdd = x => y => x + y;

        // Act
        var uncurriedAdd = curriedAdd.Uncurry();
        var result = uncurriedAdd(5, 3);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Uncurry_ThreeParameters_ShouldWork()
    {
        // Arrange
        Func<int, Func<int, Func<int, int>>> curriedAdd = x => y => z => x + y + z;

        // Act
        var uncurriedAdd = curriedAdd.Uncurry();
        var result = uncurriedAdd(1, 2, 3);

        // Assert
        Assert.Equal(6, result);
    }

    #endregion

    #region Partial Application Tests

    [Fact]
    public void Partial_TwoParameters_ShouldFixFirstArgument()
    {
        // Arrange
        Func<int, int, int> subtract = (a, b) => a - b;

        // Act
        var subtract10 = subtract.Partial(10);
        var result = subtract10(3);

        // Assert
        Assert.Equal(7, result); // 10 - 3
    }

    [Fact]
    public void Partial_ThreeParameters_OneArgument_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int> addThree = (a, b, c) => a + b + c;

        // Act
        var addWith10 = addThree.Partial(10);
        var result = addWith10(5, 3);

        // Assert
        Assert.Equal(18, result);
    }

    [Fact]
    public void Partial_ThreeParameters_TwoArguments_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int> addThree = (a, b, c) => a + b + c;

        // Act
        var addTo15 = addThree.Partial(10, 5);
        var result = addTo15(3);

        // Assert
        Assert.Equal(18, result);
    }

    [Fact]
    public void Partial_FourParameters_OneArgument_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int, string> format = (a, b, c, d) => $"{a},{b},{c},{d}";

        // Act
        var partialFormat = format.Partial(1);
        var result = partialFormat(2, 3, 4);

        // Assert
        Assert.Equal("1,2,3,4", result);
    }

    [Fact]
    public void Partial_FourParameters_TwoArguments_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int, string> format = (a, b, c, d) => $"{a},{b},{c},{d}";

        // Act
        var partialFormat = format.Partial(1, 2);
        var result = partialFormat(3, 4);

        // Assert
        Assert.Equal("1,2,3,4", result);
    }

    [Fact]
    public void Partial_FourParameters_ThreeArguments_ShouldWork()
    {
        // Arrange
        Func<int, int, int, int, string> format = (a, b, c, d) => $"{a},{b},{c},{d}";

        // Act
        var partialFormat = format.Partial(1, 2, 3);
        var result = partialFormat(4);

        // Assert
        Assert.Equal("1,2,3,4", result);
    }

    #endregion

    #region Flip Tests

    [Fact]
    public void Flip_TwoParameters_ShouldReverseArguments()
    {
        // Arrange
        Func<int, int, int> subtract = (a, b) => a - b;

        // Act
        var flippedSubtract = subtract.Flip();
        var result = flippedSubtract(3, 10);

        // Assert
        Assert.Equal(7, result); // 10 - 3
    }

    [Fact]
    public void Flip_ThreeParameters_ShouldReverseFirstTwo()
    {
        // Arrange
        Func<string, int, bool, string> format = (str, num, flag) => $"{str}-{num}-{flag}";

        // Act
        var flipped = format.Flip();
        var result = flipped(42, "test", true);

        // Assert
        Assert.Equal("test-42-True", result);
    }

    [Fact]
    public void Flip_WithDivision_ShowsNonCommutativeOperation()
    {
        // Arrange
        Func<double, double, double> divide = (a, b) => a / b;

        // Act
        var result1 = divide(10, 2);
        var result2 = divide.Flip()(10, 2);

        // Assert
        Assert.Equal(5, result1);   // 10 / 2
        Assert.Equal(0.2, result2); // 2 / 10
    }

    #endregion

    #region Identity and Constant Tests

    [Fact]
    public void Identity_ShouldReturnInput()
    {
        // Arrange
        var identity = FunctionExtensions.Identity<int>();

        // Act
        var result = identity(42);

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void Constant_WithInput_ShouldAlwaysReturnSameValue()
    {
        // Arrange
        var alwaysFive = FunctionExtensions.Constant<string, int>(5);

        // Act
        var result1 = alwaysFive("hello");
        var result2 = alwaysFive("world");

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
    }

    [Fact]
    public void Constant_Parameterless_ShouldAlwaysReturnSameValue()
    {
        // Arrange
        var alwaysHello = FunctionExtensions.Constant("hello");

        // Act
        var result1 = alwaysHello();
        var result2 = alwaysHello();

        // Assert
        Assert.Equal("hello", result1);
        Assert.Equal("hello", result2);
    }

    #endregion

    #region Apply and Invoke Tests

    [Fact]
    public void Apply_ShouldApplyValueToFunction()
    {
        // Arrange
        Func<int, int> square = x => x * x;

        // Act
        var result = 5.Apply(square);

        // Assert
        Assert.Equal(25, result);
    }

    [Fact]
    public void Apply_FluentChaining()
    {
        // Arrange
        Func<int, int> addOne = x => x + 1;
        Func<int, int> square = x => x * x;

        // Act
        var result = 5
            .Apply(addOne)
            .Apply(square);

        // Assert
        Assert.Equal(36, result); // (5 + 1)^2
    }

    [Fact]
    public void Invoke_ShouldExecuteParameterlessFunction()
    {
        // Arrange
        Func<int> getNumber = () => 42;

        // Act
        var result = getNumber.Invoke();

        // Assert
        Assert.Equal(42, result);
    }

    #endregion

    #region Predicate Tests

    [Fact]
    public void Negate_ShouldInvertPredicate()
    {
        // Arrange
        Func<int, bool> isEven = x => x % 2 == 0;

        // Act
        var isOdd = isEven.Negate();

        // Assert
        Assert.False(isOdd(4));
        Assert.True(isOdd(5));
    }

    [Fact]
    public void And_ShouldCombinePredicatesWithLogicalAnd()
    {
        // Arrange
        Func<int, bool> isPositive = x => x > 0;
        Func<int, bool> isEven = x => x % 2 == 0;

        // Act
        var isPositiveAndEven = isPositive.And(isEven);

        // Assert
        Assert.True(isPositiveAndEven(4));
        Assert.False(isPositiveAndEven(3));
        Assert.False(isPositiveAndEven(-4));
    }

    [Fact]
    public void Or_ShouldCombinePredicatesWithLogicalOr()
    {
        // Arrange
        Func<int, bool> isNegative = x => x < 0;
        Func<int, bool> isZero = x => x == 0;

        // Act
        var isNonPositive = isNegative.Or(isZero);

        // Assert
        Assert.True(isNonPositive(-5));
        Assert.True(isNonPositive(0));
        Assert.False(isNonPositive(5));
    }

    [Fact]
    public void Xor_ShouldReturnTrueWhenOnlyOnePredicateIsTrue()
    {
        // Arrange
        Func<int, bool> isEven = x => x % 2 == 0;
        Func<int, bool> isPositive = x => x > 0;

        // Act
        var xorPredicate = isEven.Xor(isPositive);

        // Assert
        Assert.True(xorPredicate(-2));  // even but not positive
        Assert.True(xorPredicate(3));   // positive but not even
        Assert.False(xorPredicate(4));  // both even and positive
        Assert.False(xorPredicate(-3)); // neither even nor positive
    }

    [Fact]
    public void PredicateComposition_ComplexScenario()
    {
        // Arrange
        Func<int, bool> isPositive = x => x > 0;
        Func<int, bool> isEven = x => x % 2 == 0;
        Func<int, bool> isLessThan100 = x => x < 100;

        // Act
        var isValid = isPositive
            .And(isEven)
            .And(isLessThan100);

        // Assert
        Assert.True(isValid(50));
        Assert.False(isValid(51));  // not even
        Assert.False(isValid(-50)); // not positive
        Assert.False(isValid(150)); // not less than 100
    }

    #endregion

    #region Action Composition Tests

    [Fact]
    public void AndThen_Actions_ShouldExecuteInOrder()
    {
        // Arrange
        var result = "";
        Action<string> action1 = x => result += x;
        Action<string> action2 = x => result += x.ToUpper();

        // Act
        var combined = action1.AndThen(action2);
        combined("hello");

        // Assert
        Assert.Equal("helloHELLO", result);
    }

    [Fact]
    public void ToFunc_Action_ShouldReturnUnit()
    {
        // Arrange
        var sideEffect = false;
        Action<int> action = x => sideEffect = true;

        // Act
        var func = action.ToFunc();
        var result = func(42);

        // Assert
        Assert.True(sideEffect);
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public void ToFunc_ParameterlessAction_ShouldReturnUnit()
    {
        // Arrange
        var sideEffect = false;
        Action action = () => sideEffect = true;

        // Act
        var func = action.ToFunc();
        var result = func();

        // Assert
        Assert.True(sideEffect);
        Assert.Equal(Unit.Value, result);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void ComplexFunctionalComposition_RealWorldScenario()
    {
        // Arrange
        Func<string, string> trim = s => s.Trim();
        Func<string, string> toLower = s => s.ToLower();
        Func<string, bool> isNotEmpty = s => !string.IsNullOrEmpty(s);
        Func<string, bool> startsWithA = s => s.StartsWith("a");

        // Act
        var processAndValidate = trim
            .AndThen(toLower)
            .AndThen(s => (s, isNotEmpty(s) && startsWithA(s)));

        var (processed, isValid) = processAndValidate("  Apple  ");

        // Assert
        Assert.Equal("apple", processed);
        Assert.True(isValid);
    }

    [Fact]
    public void CurryAndPartial_WorkTogether()
    {
        // Arrange
        Func<int, int, int, int> calculate = (a, b, c) => a * b + c;

        // Act
        var curried = calculate.Curry();
        var partialCurried = curried(2); // multiply by 2, then add
        var finalResult = partialCurried(5)(10); // (2 * 5) + 10

        // Assert
        Assert.Equal(20, finalResult);
    }

    [Fact]
    public void NullArgumentsThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((Func<int, int>)null!).Compose((int x) => x));
        
        Assert.Throws<ArgumentNullException>(() => 
            ((Func<int, int>)null!).Curry());
        
        Assert.Throws<ArgumentNullException>(() => 
            5.Apply<int, int>(null!));
        
        Assert.Throws<ArgumentNullException>(() => 
            ((Func<int, bool>)null!).Negate());
    }

    #endregion
}


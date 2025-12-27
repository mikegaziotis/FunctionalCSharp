namespace Orfe;

public partial struct Result
{
    internal static class Messages
    {
        public const string ErrorIsInaccessibleForSuccess  = "You attempted to access the Error property for a successful result. A successful result has no Error.";

        public static string ValueIsInaccessibleForFailure(string? error)
            => $"You attempted to access the Value property for a failed result. A failed result has no Value. The error was: {error}";

        public const string ErrorObjectIsNotProvidedForFailure = "You attempted to create a failure result, which must have an error, but a null error object (or empty string) was passed to the constructor.";

        public const string ErrorObjectIsProvidedForSuccess = "You attempted to create a success result, which cannot have an error, but a non-null error object was passed to the constructor.";

        public const string ValueObjectIsNotProvidedForSuccess = "You attempted to create a success result, which must have a value, but a null value object was passed to the constructor.";

        public const string ConvertFailureExceptionOnSuccess = "Failure conversion failed because the Result is in a success state.";
    }
}


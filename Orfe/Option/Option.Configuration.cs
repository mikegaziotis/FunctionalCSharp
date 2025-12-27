namespace Orfe;

public readonly partial struct Option<T>
{
    private static class Configuration
    {
        public const string NoValueException = "Option has no value.";
    }
}

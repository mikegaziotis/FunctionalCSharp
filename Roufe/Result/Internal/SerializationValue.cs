namespace Roufe;

internal struct SerializationValue<TE>
{
    public bool IsFailure { get; }
    public TE Error { get; }

    internal SerializationValue(bool isFailure, TE error)
    {
        IsFailure = isFailure;
        Error = error;
    }
}

namespace Roufe;


public partial struct Result<T, TE>
{
    public override string ToString()
    {
        return IsSuccess ? $"Success({Value})" : $"Failure({Error})";
    }
}

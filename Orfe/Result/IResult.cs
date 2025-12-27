namespace Orfe;

public interface IResult
{
    bool IsFailure { get; }
    bool IsSuccess { get; }
}

public interface IValue<out T>
{
    T Value { get; }
}

public interface IError<out TE>
{
    TE Error { get; }
}

public interface IResult<out T, out TE> : IValue<T>, IError<TE>, IResult;

public interface IResult<out T> : IResult<T, Unit>;


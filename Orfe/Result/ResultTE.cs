using System;
using System.Runtime.Serialization;
using Orfe.Internal;

namespace Orfe;

[Serializable]
public readonly partial struct Result<T, TE> : IResult<T, TE>, ISerializable
{
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;

    private readonly Option<TE> _error;
    public TE Error => ResultCommonLogic.GetErrorWithSuccessGuard(IsFailure, _error);

    private readonly Option<T> _value;
    public T Value => ResultCommonLogic.GetSuccessWithErrorGuard(IsSuccess, _value);

    internal Result(bool isFailure, TE? error, T? value)
    {
        IsFailure = ResultCommonLogic.StateGuard(isFailure, error, value);
        _error = error;
        _value = value;
    }

    private Result(SerializationInfo info, StreamingContext context)
    {
        var values = ResultCommonLogic.Deserialize<TE>(info);
        IsFailure = values.IsFailure;
        _error = values.Error;
        _value = IsFailure ? default : (T)info.GetValue("Value", typeof(T))!;
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ResultCommonLogic.GetObjectData(this, info);
    }

    public T GetValueOrDefault(T defaultValue = default!)
    {
        return IsFailure ? defaultValue : Value;
    }

    public static implicit operator Result<T, TE>(T value)
    {
        if (value is IResult<T, TE> result)
        {
            var resultError = result.IsFailure ? result.Error : default;
            var resultValue = result.IsSuccess ? result.Value : default;

            return new Result<T, TE>(result.IsFailure, resultError, resultValue);
        }

        return Result.Success<T, TE>(value);
    }

    public static implicit operator Result<T, TE>(TE error)
    {
        if (error is IResult<T, TE> result)
        {
            var resultError = result.IsFailure ? result.Error : default;
            var resultValue = result.IsSuccess ? result.Value : default;

            return new Result<T, TE>(result.IsFailure, resultError, resultValue);
        }

        return Result.Failure<T, TE>(error);
    }

    public static implicit operator Result<Unit,TE>(Result<T, TE> result)
        => result.IsSuccess
            ? Result.Success<Unit,TE>(Unit.Value)
            : Result.Failure<Unit,TE>(result.Error);

    public static implicit operator Result<Unit,string>(Result<T, TE> result)
        => result.IsSuccess
            ? Result.Success<Unit,string>(Unit.Value)
            : Result.Failure<Unit,string>(result.Error!.ToString());
}

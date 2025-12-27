using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orfe.Internal;

internal static class ResultCommonLogic
{
    // ReSharper disable once MemberCanBePrivate.Global
    internal static void GetObjectDataCommon(IResult result, SerializationInfo info)
    {
        info.AddValue("IsFailure", result.IsFailure);
        info.AddValue("IsSuccess", result.IsSuccess);
    }

    internal static void GetObjectData<T, TE>(Result<T, TE> result, SerializationInfo info)
    {
        GetObjectDataCommon(result, info);
        if (result.IsFailure)
        {
            info.AddValue("Error", result.Error);
        }

        if (result.IsSuccess)
        {
            info.AddValue("Value", result.Value);
        }
    }

    internal static bool StateGuard<T, TE>(bool isFailure, TE? error, T? value)
    {
        if (isFailure)
        {
            if (error is null || (error is string && error.Equals(string.Empty)))
                throw new ArgumentNullException(nameof(error), Result.Messages.ErrorObjectIsNotProvidedForFailure);
        }
        else
        {
            if (error is null || !EqualityComparer<TE>.Default.Equals(error, default))
                throw new ArgumentException(Result.Messages.ErrorObjectIsProvidedForSuccess, nameof(error));

            if (value is null || !EqualityComparer<T>.Default.Equals(value, default))
                throw  new ArgumentNullException(nameof(value), Result.Messages.ValueObjectIsNotProvidedForSuccess);
        }

        return isFailure;
    }

    internal static TE GetErrorWithSuccessGuard<TE>(bool isFailure, Option<TE> error)
        => isFailure
            ? error.Match(
                some: e => e,
                none: () =>  throw new ResultSuccessException())
            : throw new ResultSuccessException();

    internal static T GetSuccessWithErrorGuard<T>(bool isSuccess, Option<T> value)
        => isSuccess
            ? value.Match(
                some: t => t,
                none: () =>  throw new ResultSuccessException())
            : throw new ResultSuccessException();

    internal static SerializationValue<string> Deserialize(SerializationInfo info) => Deserialize<string>(info);

    internal static SerializationValue<TE> Deserialize<TE>(SerializationInfo info)
    {
        var isFailure = info.GetBoolean("IsFailure");
        var error = isFailure ? (TE)info.GetValue("Error", typeof(TE))! : default;
        return error is not null
            ? new SerializationValue<TE>(isFailure, error)
            : throw new SerializationException("Deserialization failed: Error object is missing.");
    }
}

// ReSharper disable UnusedAutoPropertyAccessor.Global
using System;

namespace Roufe;

public class ResultFailureException : Exception
{
    public string Error { get; }

    internal ResultFailureException(string error) : base(Result.Messages.ValueIsInaccessibleForFailure(error))
    {
        Error = error;
    }
}

public class ResultFailureException<TE> : ResultFailureException
{

    public new TE Error { get; }

    internal ResultFailureException(TE error) : base(Result.Messages.ValueIsInaccessibleForFailure(error?.ToString()))
    {
        Error = error;
    }
}

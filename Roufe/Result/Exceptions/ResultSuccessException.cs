using System;

namespace Roufe;

public class ResultSuccessException : Exception
{
    internal ResultSuccessException() : base(Result.Messages.ErrorIsInaccessibleForSuccess)
    {
    }
}

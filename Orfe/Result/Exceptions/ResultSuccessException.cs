using System;

namespace Orfe;

public class ResultSuccessException : Exception
{
    internal ResultSuccessException() : base(Result.Messages.ErrorIsInaccessibleForSuccess)
    {
    }
}

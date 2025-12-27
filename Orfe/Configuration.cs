using System;

namespace Orfe;

public static class Configuration
{

    public static string ErrorMessagesSeparator = ", ";

    public static bool DefaultConfigureAwait = false;

    public static Func<Exception, string> DefaultTryErrorHandler = exc => exc.Message;

}

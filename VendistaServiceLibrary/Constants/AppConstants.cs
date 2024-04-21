using System;

namespace VendistaServiceLibrary.Constants;

internal static class AppConstants
{
    public const string VendistaPathGetToken = "/token";
    public const string VendistaPathGetTerminalCommands = "/commands/types";
    public const string VendistaPathGetTerminalLog = "/terminals/{0}/commands";
    public const string VendistaPathSendTerminalCommand = "/terminals/commands";

    public const string ParameterNameToken = "token";
    public const string ParameterNameLogin = "login";
    public const string ParameterNamePassword = "password";

}

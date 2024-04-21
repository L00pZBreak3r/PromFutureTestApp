using System.Collections.Generic;
using System.Threading.Tasks;

using PromFutureTestLibrary.DataObjects;

namespace VendistaServiceLibrary.Services;

public interface IVendistaSwaggerService
{
    string Token { get; set; }
    Task<TerminalCommandInfo[]> GetTerminalCommandsAsync();
    Task<string> GetTokenAsync(string login, string password);
    Task<LogTableRow[]> GetTerminalLogAsync(int terminal_id, IDictionary<int, TerminalCommandInfo>? aAvailableTerminalCommands);
    Task<TerminalCommandSendResult> SendTerminalCommandAsync(int[] aTerminalIds, int aCommandId, int[] aIntegerParameters, string?[] aStringParameters, IDictionary<int, TerminalCommandInfo>? aAvailableTerminalCommands);
}

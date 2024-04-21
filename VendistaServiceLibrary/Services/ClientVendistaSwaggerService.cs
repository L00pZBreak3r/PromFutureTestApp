using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using VendistaServiceLibrary.Constants;
using VendistaServiceLibrary.DataObjects;
using PromFutureTestLibrary.DataObjects;

namespace VendistaServiceLibrary.Services;

public class ClientVendistaSwaggerService(HttpClient mHttpClient) : IVendistaSwaggerService
{
    public string Token { get; set; } = string.Empty;

    public async Task<TerminalCommandInfo[]> GetTerminalCommandsAsync()
    {
        TerminalCommandInfo[] aResult = Array.Empty<TerminalCommandInfo>();
        if (!string.IsNullOrEmpty(Token))
        {
            var aVendistaTerminalCommandsList = await mHttpClient.GetFromJsonAsync<VendistaTerminalCommandListInfo>(AppConstants.VendistaPathGetTerminalCommands + $"?{AppConstants.ParameterNameToken}={Token}");

            if ((aVendistaTerminalCommandsList is not null) && (aVendistaTerminalCommandsList.items.Length > 0))
            {
                aResult = new TerminalCommandInfo[aVendistaTerminalCommandsList.items.Length];
                List<IntegerParameterInfo> aIntegerParameters = new();
                List<StringParameterInfo> aStringParameters = new();
                int i = 0;
                foreach (var aItem in aVendistaTerminalCommandsList.items)
                {
                    aIntegerParameters.Clear();
                    aStringParameters.Clear();
                    if (!string.IsNullOrEmpty(aItem.parameter_name1))
                    {
                        aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name1, aItem.parameter_default_value1 ?? 0));
                        if (!string.IsNullOrEmpty(aItem.parameter_name2))
                        {
                            aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name2, aItem.parameter_default_value2 ?? 0));
                            if (!string.IsNullOrEmpty(aItem.parameter_name3))
                            {
                                aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name3, aItem.parameter_default_value3 ?? 0));
                                if (!string.IsNullOrEmpty(aItem.parameter_name4))
                                {
                                    aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name4, aItem.parameter_default_value4 ?? 0));
                                    if (!string.IsNullOrEmpty(aItem.parameter_name5))
                                    {
                                        aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name5, aItem.parameter_default_value5 ?? 0));
                                        if (!string.IsNullOrEmpty(aItem.parameter_name6))
                                        {
                                            aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name6, aItem.parameter_default_value6 ?? 0));
                                            if (!string.IsNullOrEmpty(aItem.parameter_name7))
                                            {
                                                aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name7, aItem.parameter_default_value7 ?? 0));
                                                if (!string.IsNullOrEmpty(aItem.parameter_name8))
                                                {
                                                    aIntegerParameters.Add(new IntegerParameterInfo(aItem.parameter_name8, aItem.parameter_default_value8 ?? 0));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(aItem.str_parameter_name1))
                    {
                        aStringParameters.Add(new StringParameterInfo(aItem.str_parameter_name1, aItem.str_parameter_default_value1 ?? string.Empty));
                        if (!string.IsNullOrEmpty(aItem.str_parameter_name2))
                        {
                            aStringParameters.Add(new StringParameterInfo(aItem.str_parameter_name2, aItem.str_parameter_default_value2 ?? string.Empty));
                        }
                    }

                    aResult[i++] = new TerminalCommandInfo(aItem.id, aItem.name, aItem.visible, aIntegerParameters.ToArray(), aStringParameters.ToArray());
                }
            }
        }

        return aResult;
    }

    public async Task<string> GetTokenAsync(string login, string password)
    {
        string aResult = string.Empty;
        if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
        {
            var aVendistaToken = await mHttpClient.GetFromJsonAsync<VendistaTokenInfo>(AppConstants.VendistaPathGetToken + $"?{AppConstants.ParameterNameLogin}={login}&{AppConstants.ParameterNamePassword}={password}");
            if (aVendistaToken is not null)
                aResult = aVendistaToken.token;
        }
        return aResult;
    }

    public async Task<LogTableRow[]> GetTerminalLogAsync(int terminal_id, IDictionary<int, TerminalCommandInfo>? aAvailableTerminalCommands)
    {
        LogTableRow[] aResult = Array.Empty<LogTableRow>();
        if (!string.IsNullOrEmpty(Token))
        {
            var aVendistaTerminalLogTable = await mHttpClient.GetFromJsonAsync<VendistaTerminalLogTable>(string.Format(AppConstants.VendistaPathGetTerminalLog, terminal_id) + $"?{AppConstants.ParameterNameToken}={Token}");

            if ((aVendistaTerminalLogTable is not null) && (aVendistaTerminalLogTable.items.Length > 0))
            {
                aResult = new LogTableRow[aVendistaTerminalLogTable.items.Length];

                int i = 0;
                foreach (var aItem in aVendistaTerminalLogTable.items.OrderByDescending(a => a.time_created))
                {
                    string aCommandName = $"Command{aItem.command_id}";
                    if ((aAvailableTerminalCommands is not null) && aAvailableTerminalCommands.TryGetValue(aItem.command_id, out var aCommandInfo))
                        aCommandName = aCommandInfo.Name;
                    aResult[i++] = new LogTableRow(i, aItem.terminal_id, aItem.user_name, aItem.time_created, aItem.command_id, aCommandName, aItem.parameter1, aItem.parameter2, aItem.parameter3, aItem.state, aItem.state_name);
                }
            }
        }
        return aResult;
    }

    public async Task<TerminalCommandSendResult> SendTerminalCommandAsync(int[] aTerminalIds, int aCommandId, int[] aIntegerParameters, string?[] aStringParameters, IDictionary<int, TerminalCommandInfo>? aAvailableTerminalCommands)
    {
        TerminalCommandSendResult aResult = new(Array.Empty<int>(), 0, string.Empty, null, null, 0, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, null, null, 0, string.Empty);
        if (aTerminalIds.Length > 0)
        {
            VendistaTerminalCommandSendMultipleInfo aSendMultipleInfo = new(aTerminalIds, aCommandId,
                (aIntegerParameters.Length > 0) ? aIntegerParameters[0] : 0,
                (aIntegerParameters.Length > 1) ? aIntegerParameters[1] : 0,
                (aIntegerParameters.Length > 2) ? aIntegerParameters[2] : 0,
                (aIntegerParameters.Length > 3) ? aIntegerParameters[3] : 0,
                (aIntegerParameters.Length > 4) ? aIntegerParameters[4] : 0,
                (aIntegerParameters.Length > 5) ? aIntegerParameters[5] : 0,
                (aIntegerParameters.Length > 6) ? aIntegerParameters[6] : 0,
                (aIntegerParameters.Length > 7) ? aIntegerParameters[7] : 0,
                (aStringParameters.Length  > 0) ? aStringParameters[0] : null,
                (aStringParameters.Length  > 1) ? aStringParameters[1] : null);
            VendistaTerminalCommandSendInfo aSendSingleInfo = aSendMultipleInfo;

            using var aResponse = (aTerminalIds.Length == 1) ? await mHttpClient.PostAsJsonAsync(string.Format(AppConstants.VendistaPathGetTerminalLog, aTerminalIds[0]) + $"?{AppConstants.ParameterNameToken}={Token}", aSendSingleInfo) : await mHttpClient.PostAsJsonAsync(AppConstants.VendistaPathSendTerminalCommand + $"?{AppConstants.ParameterNameToken}={Token}", aSendMultipleInfo);
            VendistaTerminalCommandSendResultInfo? aResultInfo = await aResponse.Content.ReadFromJsonAsync<VendistaTerminalCommandSendResultInfo>();
            if ((aResultInfo is not null) && aResultInfo.success && (aResultInfo.item is not null))
            {
                var aItem = aResultInfo.item;
                string aCommandName = $"Command{aItem.command_id}";
                if ((aAvailableTerminalCommands is not null) && aAvailableTerminalCommands.TryGetValue(aItem.command_id, out var aCommandInfo))
                    aCommandName = aCommandInfo.Name;
                aResult = new(aItem.ids, aItem.terminal_id, aItem.user_name, aItem.time_created, aItem.time_delivered, aItem.command_id, aCommandName,
                    aItem.parameter1,
                    aItem.parameter2,
                    aItem.parameter3,
                    aItem.parameter4,
                    aItem.parameter5,
                    aItem.parameter6,
                    aItem.parameter7,
                    aItem.parameter8,
                    aItem.str_parameter1,
                    aItem.str_parameter2,
                    aItem.state, aItem.state_name);
            }
        }
        return aResult;
    }
}

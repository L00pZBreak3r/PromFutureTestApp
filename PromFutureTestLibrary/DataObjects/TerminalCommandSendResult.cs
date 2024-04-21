namespace PromFutureTestLibrary.DataObjects;

public record TerminalCommandSendResult(int[] Ids, int TerminalId, string? UserName, string? TimeCreated, string? TimeDelivered, int CommandId, string CommandName, int Parameter1, int Parameter2, int Parameter3, int Parameter4, int Parameter5, int Parameter6, int Parameter7, int Parameter8, string? StringParameter1, string? StringParameter2, int State, string StateName)
    : LogTableRowBase(TerminalId, UserName, TimeCreated, CommandId, CommandName, Parameter1, Parameter2, Parameter3, State, StateName);

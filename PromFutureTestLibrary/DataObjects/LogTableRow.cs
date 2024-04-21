namespace PromFutureTestLibrary.DataObjects;

public record LogTableRow(int Number, int TerminalId, string? UserName, string? TimeCreated, int CommandId, string CommandName, int Parameter1, int Parameter2, int Parameter3, int State, string StateName)
    : LogTableRowBase(TerminalId, UserName, TimeCreated, CommandId, CommandName, Parameter1, Parameter2, Parameter3, State, StateName);

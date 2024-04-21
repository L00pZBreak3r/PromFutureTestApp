namespace PromFutureTestLibrary.DataObjects;

public abstract record LogTableRowBase(int TerminalId, string? UserName, string? TimeCreated, int CommandId, string CommandName, int Parameter1, int Parameter2, int Parameter3, int State, string StateName);

namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalCommandSendResultInfo(bool success, string? error, VendistaTerminalCommandSendResultItem? item);

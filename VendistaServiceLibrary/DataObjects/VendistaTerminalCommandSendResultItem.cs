using System;

namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalCommandSendResultItem(int[] ids, int terminal_id, string? user_name, int command_id, int state, string state_name, string? time_created, string? time_delivered, int id,
    int parameter1,
    int parameter2,
    int parameter3,
    int parameter4,
    int parameter5,
    int parameter6,
    int parameter7,
    int parameter8,
    string? str_parameter1,
    string? str_parameter2
    ) : VendistaTerminalLogTableRow(terminal_id, user_name, command_id, state, state_name, time_created, time_delivered, id,
    parameter1,
    parameter2,
    parameter3,
    parameter4,
    parameter5,
    parameter6,
    parameter7,
    parameter8,
    str_parameter1,
    str_parameter2
    );

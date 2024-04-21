using System;

namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalCommandSendMultipleInfo(int[] ids, int command_id,
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
    ) : VendistaTerminalCommandSendInfo(command_id,
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

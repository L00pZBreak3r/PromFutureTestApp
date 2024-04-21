using System;

namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalCommandSendInfo(int command_id,
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
    );

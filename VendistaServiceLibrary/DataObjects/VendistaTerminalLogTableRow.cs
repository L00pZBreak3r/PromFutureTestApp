namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalLogTableRow(int terminal_id, string? user_name, int command_id, int state, string state_name, string? time_created, string? time_delivered, int id,
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

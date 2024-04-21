namespace VendistaServiceLibrary.DataObjects;

internal record VendistaTerminalCommandInfo(int id, string name, bool visible,
    string? parameter_name1,
    string? parameter_name2,
    string? parameter_name3,
    string? parameter_name4,
    string? parameter_name5,
    string? parameter_name6,
    string? parameter_name7,
    string? parameter_name8,
    string? str_parameter_name1,
    string? str_parameter_name2,
    int? parameter_default_value1,
    int? parameter_default_value2,
    int? parameter_default_value3,
    int? parameter_default_value4,
    int? parameter_default_value5,
    int? parameter_default_value6,
    int? parameter_default_value7,
    int? parameter_default_value8,
    string? str_parameter_default_value1,
    string? str_parameter_default_value2
    );

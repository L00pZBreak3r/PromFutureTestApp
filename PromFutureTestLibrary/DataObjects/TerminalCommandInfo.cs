using System;

namespace PromFutureTestLibrary.DataObjects;

public record TerminalCommandInfo(int Id, string Name, bool Visible, IntegerParameterInfo[] IntegerParameters, StringParameterInfo[] StringParameters);

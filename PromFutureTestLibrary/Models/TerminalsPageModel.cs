using System;
using System.Collections.Generic;
using System.Linq;

using PromFutureTestLibrary.DataObjects;

namespace PromFutureTestLibrary.Models;

public sealed class TerminalsPageModel
{
    private const int MaxIntegerParameterCount = 8;
    private const int MaxStringParameterCount = 2;
    private static readonly char[] TerminalIdSeparators = new[] { ' ', '\t', '\n', '\r', ',', ';', '.', ':' };

    public Dictionary<int, TerminalCommandInfo> AvailableTerminalCommands { get; } = new();

    private string mTerminalList = string.Empty;
    public string TerminalList
    {
        get => mTerminalList;
        set
        {
            if (mTerminalList != value)
            {
                mTerminalList = value;
                SetTerminalIds();
            }
        }
    }

    private int[] mTerminalIds = [];
    public int[] TerminalIds => mTerminalIds;

    public int SelectedTerminalCommandId { get; set; }

    public int[] IntegerParameters { get; } = new int[MaxIntegerParameterCount];
    public string?[] StringParameters { get; } = new string?[MaxStringParameterCount];

    public int IntegerParameter1 { get; set; }
    public int IntegerParameter2 { get; set; }
    public int IntegerParameter3 { get; set; }
    public int IntegerParameter4 { get; set; }
    public int IntegerParameter5 { get; set; }
    public int IntegerParameter6 { get; set; }
    public int IntegerParameter7 { get; set; }
    public int IntegerParameter8 { get; set; }

    public string StringParameter1 { get; set; } = string.Empty;
    public string StringParameter2 { get; set; } = string.Empty;

    public event Action? TerminalIdsChanged;

    private static int ParseTerminalId(string aValue)
    {
        int.TryParse(aValue, out int aResult);
        return aResult;
    }

    private void SetTerminalIds()
    {
        mTerminalIds = Array.Empty<int>();
        if (!string.IsNullOrWhiteSpace(TerminalList))
        {
            string[] aIdsStr = TerminalList.Split(TerminalIdSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            mTerminalIds = aIdsStr.Select(ParseTerminalId).Where(a => a > 0).Distinct().ToArray();
        }

        TerminalIdsChanged?.Invoke();
    }

    private void SetParameters()
    {
        IntegerParameter1 = IntegerParameters[0];
        IntegerParameter2 = IntegerParameters[1];
        IntegerParameter3 = IntegerParameters[2];
        IntegerParameter4 = IntegerParameters[3];
        IntegerParameter5 = IntegerParameters[4];
        IntegerParameter6 = IntegerParameters[5];
        IntegerParameter7 = IntegerParameters[6];
        IntegerParameter8 = IntegerParameters[7];
        StringParameter1 = StringParameters[0] ?? string.Empty;
        StringParameter2 = StringParameters[1] ?? string.Empty;
    }

    public void GetParameters()
    {
        IntegerParameters[0] = IntegerParameter1;
        IntegerParameters[1] = IntegerParameter2;
        IntegerParameters[2] = IntegerParameter3;
        IntegerParameters[3] = IntegerParameter4;
        IntegerParameters[4] = IntegerParameter5;
        IntegerParameters[5] = IntegerParameter6;
        IntegerParameters[6] = IntegerParameter7;
        IntegerParameters[7] = IntegerParameter8;
        StringParameters[0] = StringParameter1;
        StringParameters[1] = StringParameter2;
    }

    private void ClearParameters()
    {
        Array.Clear(IntegerParameters);
        Array.Clear(StringParameters);
    }

    public void Update(IEnumerable<TerminalCommandInfo> aAvailableTerminalCommands)
    {
        AvailableTerminalCommands.Clear();
        ClearParameters();
        SetParameters();
        TerminalList = string.Empty;
        SelectedTerminalCommandId = 0;

        foreach (var aItem in aAvailableTerminalCommands)
        {
            AvailableTerminalCommands.Add(aItem.Id, aItem);
        }

    }

    public TerminalCommandInfo? GetSelectedTerminalCommand()
    {
        AvailableTerminalCommands.TryGetValue(SelectedTerminalCommandId, out var aResult);
        ClearParameters();
        if (aResult is not null)
        {
            int aParameterCount = aResult.IntegerParameters.Length;
            if (aParameterCount > MaxIntegerParameterCount)
                aParameterCount = MaxIntegerParameterCount;
            for (int i = 0; i < aParameterCount; i++)
                IntegerParameters[i] = aResult.IntegerParameters[i].DefaultValue;

            aParameterCount = aResult.StringParameters.Length;
            if (aParameterCount > MaxStringParameterCount)
                aParameterCount = MaxStringParameterCount;
            for (int i = 0; i < aParameterCount; i++)
                StringParameters[i] = aResult.StringParameters[i].DefaultValue;
        }
        SetParameters();
        return aResult;
    }
}

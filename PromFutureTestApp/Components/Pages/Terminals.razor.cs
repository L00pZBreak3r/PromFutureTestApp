using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

using PromFutureTestLibrary.DataObjects;
using PromFutureTestLibrary.Models;
using VendistaServiceLibrary.Services;

using PromFutureTestApp.Constants;

namespace PromFutureTestApp.Components.Pages;

public partial class Terminals
{
    private const string Title = AppConstants.Title;

    private const string NoTokenText = "Получите сначала Vendista token";
    private const string NoAvailableTerminalCommandsText = "Не удалось получить список команд от Vendista";

    private const string TitleTerminalId = "ID терминала(ов)";
    private const string TitleTerminalCommand = "Команда";
    private const string ButtonTitleGoBack = "Назад";
    private const string ButtonTitleSend = "Отправить";

    private const string TerminalListHint = "Список ID терминалов";
    private const string TerminalCommandHint = "Выберите команду";


    private const string LocalStorageTokenFieldName = AppConstants.LocalStorageTokenFieldName;

    private string? TokenFromLocalStorage { get; set; }
    private bool ItemExist { get; set; }

    private string ErrorText { get; set; } = NoTokenText;

    private string[] LogTableColumnTitles { get; } =
        {
            "№",
            "Дата и время",
            "Команда",
            "Параметр 1",
            "Параметр 2",
            "Параметр 3",
            "Статус",
        };

    public TerminalsPageModel PageModel { get; } = new();

    private TerminalCommandInfo[] AvailableTerminalCommands { get; set; } = [];
    private IntegerParameterInfo[] IntegerParameterList { get; set; } = [];
    private StringParameterInfo[] StringParameterList { get; set; } = [];

    private LogTableRow[] TerminalLogTableRows { get; set; } = [];

    protected override void OnInitialized()
    {
        PageModel.TerminalIdsChanged += PageModel_TerminalIdsChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            bool aItemExist = await GetTokenFromLocalStorage();
            if (aItemExist)
            {
                mVendistaSwaggerService.Token = TokenFromLocalStorage!;
                var aAvailableTerminalCommands = await mVendistaSwaggerService.GetTerminalCommandsAsync();
                if (aAvailableTerminalCommands.Length > 0)
                    aItemExist = Update(aAvailableTerminalCommands);
                if (!aItemExist)
                    ErrorText = NoAvailableTerminalCommandsText;
            }
            else
                ErrorText = NoTokenText;

            ItemExist = aItemExist;

            StateHasChanged();
        }
    }

    private void SetCurrentTerminalCommand(int aCommandId)
    {
        PageModel.SelectedTerminalCommandId = aCommandId;
        var aSelectedCommand = PageModel.GetSelectedTerminalCommand();
        if (aSelectedCommand is null)
        {
            IntegerParameterList = [];
            StringParameterList = [];
        }
        else
        {
            IntegerParameterList = aSelectedCommand.IntegerParameters;
            StringParameterList = aSelectedCommand.StringParameters;
        }
    }

    private bool Update(IEnumerable<TerminalCommandInfo> aAvailableTerminalCommands)
    {
        bool aResult = false;
        PageModel.Update(aAvailableTerminalCommands);
        AvailableTerminalCommands = PageModel.AvailableTerminalCommands.Values.Where(a => a.Visible).ToArray();
        IntegerParameterList = [];
        StringParameterList = [];

        if (AvailableTerminalCommands.Length > 0)
        {
            SetCurrentTerminalCommand(AvailableTerminalCommands[0].Id);
            aResult = true;
        }
        return aResult;
    }

    private async Task Send()
    {
        PageModel.GetParameters();
        var aSendResult = await mVendistaSwaggerService.SendTerminalCommandAsync(PageModel.TerminalIds, PageModel.SelectedTerminalCommandId, PageModel.IntegerParameters, PageModel.StringParameters, PageModel.AvailableTerminalCommands);
        var aList = await TerminalIdsChangedActionAsync();
        TerminalLogTableRows = aList.ToArray();
    }

    private void TerminalCommandChanged(ChangeEventArgs e)
    {
        SetCurrentTerminalCommand((e.Value is string aStringValue && int.TryParse(aStringValue, out int aValue)) ? aValue : 0);
    }

    private async Task<List<LogTableRow>> TerminalIdsChangedActionAsync()
    {
        List<LogTableRow> aTerminalLogTableRows = new();
        foreach (int aTerminalId in PageModel.TerminalIds)
        {
            var aRows = await mVendistaSwaggerService.GetTerminalLogAsync(aTerminalId, PageModel.AvailableTerminalCommands);
            aTerminalLogTableRows.AddRange(aRows);
        }
        return aTerminalLogTableRows;
    }

    private void PageModel_TerminalIdsChanged()
    {
        var aList = Task.Run(TerminalIdsChangedActionAsync).Result;
        TerminalLogTableRows = aList.ToArray();
    }


    private async Task<bool> GetTokenFromLocalStorage()
    {
        bool aResult = false;

        try
        {
            TokenFromLocalStorage = await mLocalStorage.GetItemAsync<string>(LocalStorageTokenFieldName);

            aResult = !string.IsNullOrEmpty(TokenFromLocalStorage);
        }
        catch(Exception)
        {
            Console.WriteLine($"error reading '{LocalStorageTokenFieldName}'");
        }

        return aResult;
    }

}
using VendistaServiceLibrary.Services;
using PromFutureTestApp.Constants;

namespace PromFutureTestApp.Components.Pages;

public partial class Home
{
    public const string Title = "Регистрация пользователя в Vendista";
    public const string NavTitle = "Регистрация";

    private const string RegisterDivTitle = "Регистрация (если еще нет Vendista token)";
    private const string TokenDivTitle = "Vendista token (если уже есть)";
    private const string RemoveItemDivTitle = "Удалить Vendista token";
    private const string ClearLocalStorageDivTitle = "Очистить LocalStorage";

    private const string ValueReadDivTitle = "Сохраненный Vendista token:";

    private const string RegisterButtonTitle = "Регистрация";
    private const string TokenButtonTitle = "Сохранить";
    private const string RemoveItemButtonTitle = "Удалить";
    private const string ClearLocalStorageButtonTitle = "Очистить";

    private const string UserNameHint = "логин";
    private const string UserPasswordHint = "пароль";
    private const string TokenHint = "token";

    private const string LocalStorageTokenFieldName = AppConstants.LocalStorageTokenFieldName;

    private string? TokenFromLocalStorage { get; set; }
    private string? StringFromLocalStorage { get; set; }
    private int ItemsInLocalStorage { get; set; }

    private string UserName { get; set; } = AppConstants.VendistaUserDefault;
    private string UserPassword { get; set; } = AppConstants.VendistaPasswordDefault;
    private string? Token { get; set; }

    private bool ItemExist { get; set; }
    IEnumerable<string> Keys { get; set; } = new List<string>();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        Keys = await mLocalStorage.KeysAsync();

        if (firstRender)
        {
            await GetTokenFromLocalStorage();
            await GetLocalStorageLength();

            mLocalStorage.Changed += (_, e) =>
            {
                Console.WriteLine($"Value for key {e.Key} changed from {e.OldValue} to {e.NewValue}");
            };

            StateHasChanged();
        }
    }

    private async Task UpdateTokenInLocalStorageAsync(string aToken)
    {
        await mLocalStorage.SetItemAsync(LocalStorageTokenFieldName, aToken);
        await GetTokenFromLocalStorage();
    }

    private async Task RegisterUser()
    {
        if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(UserPassword))
        {
            string aToken = await mVendistaSwaggerService.GetTokenAsync(UserName, UserPassword);
            await UpdateTokenInLocalStorageAsync(aToken);
        }
    }

    private async Task SaveToken()
    {
        await UpdateTokenInLocalStorageAsync(Token!);
        await GetLocalStorageLength();

        Token = "";
    }

    private async Task GetTokenFromLocalStorage()
    {
        try
        {
            TokenFromLocalStorage = await mLocalStorage.GetItemAsync<string>(LocalStorageTokenFieldName);
        }
        catch(Exception)
        {
            Console.WriteLine($"error reading '{LocalStorageTokenFieldName}'");
        }
    }


    private async Task RemoveItem()
    {
        await mLocalStorage.RemoveItemAsync(LocalStorageTokenFieldName);
        await GetTokenFromLocalStorage();
        await GetLocalStorageLength();
    }

    private async Task ClearLocalStorage()
    {
        Console.WriteLine("Calling Clear...");
        await mLocalStorage.ClearAsync();
        Console.WriteLine($"Getting '{LocalStorageTokenFieldName}' from local storage...");
        await GetTokenFromLocalStorage();
        Console.WriteLine("Calling Get Length...");
        await GetLocalStorageLength();
    }

    private async Task GetLocalStorageLength()
    {
        Console.WriteLine(await mLocalStorage.LengthAsync());
        ItemsInLocalStorage = await mLocalStorage.LengthAsync();
        ItemExist = await mLocalStorage.ContainKeyAsync(LocalStorageTokenFieldName);
    }

}
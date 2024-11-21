using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class RadioButtonPage : IBasePage
{
    public IPage? Page { get; set; }
    public string ExpectedTitle { get; } = "Radio Button";
    public string Url => "https://demoqa.com/radio-button";
    private ILocator Title => Page!.Locator("//h1[contains(text(),'Radio Button')]");
    private ILocator YesRadio => Page!.Locator("//label[@class='custom-control-label' and @for='yesRadio']");
    private ILocator Output => Page!.Locator("//p[@class='mt-3']");
    public async Task<RadioButtonPage> Open()
    {
        await Page!.GotoAsync(Url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        return this;
    }

    public async Task NavigateToPageAsync()
    {
        await Page!.GotoAsync(Url);
    }

    public async Task<bool> VerifyTitleAsync()
    {
        return await Title.TextContentAsync() == ExpectedTitle;
    }

    public async Task ClickYesRadioAsync()
    {
        await YesRadio.ClickAsync();
    }

    public async Task<string> GetOutputTextAsync()
    {
        return await Output.TextContentAsync();
    }

}
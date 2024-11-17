using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public partial class DynamicPropertiesPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/dynamic-properties";
    public string ExpectedTitle { get; } = "Dynamic Properties";

    public ILocator Title => Page!.Locator("xpath=//h1[text()='Dynamic Properties']");
    public ILocator EnableButton => Page!.Locator("//button[@id='enableAfter']");
    public ILocator ColorButton => Page!.Locator("//button[@id='colorChange']");
    public ILocator VisibleAfterButton => Page!.Locator("//button[@id='visibleAfter']");
    
    public async Task<DynamicPropertiesPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
    public async Task<string> CheckColor(ILocator button)
    {
        var color = await button.EvaluateAsync<string>("el => getComputedStyle(el).color");
        return color;
    }
}
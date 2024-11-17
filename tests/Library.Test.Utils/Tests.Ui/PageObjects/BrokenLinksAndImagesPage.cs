using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public partial class BrokenLinksAndImagesPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/broken";
    public string ExpectedTitle { get; } = "Broken Links - Images";

    public ILocator Title => Page!.Locator("xpath=//h1[text()='Broken Links - Images']");
    public ILocator ValidLink => Page!.Locator("//a[@href='http://demoqa.com' and text()='Click Here for Valid Link']");
    public ILocator BrokenLink => Page!.Locator("//a[text()='Click Here for Broken Link']");
    public ILocator Elements => Page!.Locator("//div[@class='card mt-4 top-card']/div/div/h5[contains(text(),'Elements')]");
    public ILocator BrokenSiteText => Page!.Locator("//div[@class='example']/p[1]");
    
    public async Task<BrokenLinksAndImagesPage> Open()
    {
        await Page!.GotoAsync(Url);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        return this;
    }

    public async Task<bool> CheckImageValidity(string imageLocator)
    {
        await Page!.GotoAsync("https://demoqa.com/images/" + imageLocator);
        await Page!.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var imgXPath = $"//img[contains(@src, '{imageLocator}')]";
        var imgCount = await Page.Locator(imgXPath).CountAsync();

        return imgCount > 0;
    }

}
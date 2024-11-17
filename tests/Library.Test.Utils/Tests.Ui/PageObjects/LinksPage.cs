using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public partial class LinksPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/links";
    public string ExpectedTitle { get; } = "Links";

    public ILocator Title => Page!.Locator("xpath=//h1[text()='Links']");
    public ILocator CreatedLink => Page!.Locator("id=created");
    public ILocator ResultText => Page!.Locator("id=linkResponse");

    public async Task<LinksPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
}
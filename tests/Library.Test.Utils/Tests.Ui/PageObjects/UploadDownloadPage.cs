using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public partial class UploadDownloadPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/upload-download";
    public string ExpectedTitle { get; } = "Upload and Download";

    public ILocator Title => Page!.Locator("xpath=//h1[text()='Upload and Download']");
    public ILocator DownloadButton => Page!.Locator("id=downloadButton");
    public ILocator UploadButton => Page!.Locator("//input[@id='uploadFile']");
    public ILocator FilePath => Page!.Locator("//p[@id='uploadedFilePath']");
    public async Task<UploadDownloadPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
}
using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class UploadDownloadPageTests
{
    private UploadDownloadPage Page { get; set; }
    private readonly BrowserSetUpBuilder Browser = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await Browser
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithSlowMo(100)
            .WithTimeout(10000)
            .WithVideoSize(1900, 1080)
            .SaveVideo("videos/")
            .WithArgs("--start-maximized")
            .OpenNewPage<UploadDownloadPage>();
        Browser.AddRequestResponseLogger();
        Page!.Open();
    }

    [SetUp]
    public async Task SetUp()
    {
        var traceName = TestContext.CurrentContext.Test.ClassName + "/" + TestContext.CurrentContext.Test.Name;
        await Browser.StartTracing(traceName);
    }

    [Test]
    public async Task OpenPage()
    {
        var title = await Page!.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo(Page!.ExpectedTitle));
    }

    [Test]
    public async Task CheckDownLoad()
    {
        var download = await Page.Page.RunAndWaitForDownloadAsync(async () =>
        {
            await Page.DownloadButton.ClickAsync();
        });

        Assert.That(download, Is.Not.Null, "Download did not start.");

        var path = await download.PathAsync();
        Assert.That(File.Exists(path), Is.True, "Downloaded file does not exist.");
    }

    [Test]
    public async Task CheckUpload()
    {
        var filePath = "C:/Hillel/homework_21/sampleFile.jpeg";
        await Page.UploadButton.ClickAsync();

        var fileInput = Page.Page.Locator("input[type='file']");
        await fileInput.SetInputFilesAsync(filePath);

        var uploadedFilePath = await Page.FilePath.InnerTextAsync();
        Assert.That(uploadedFilePath, Is.EqualTo("C:\\fakepath\\sampleFile.jpeg"), "Uploaded file path is incorrect.");
    }

    [TearDown]
    public async Task TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            await Browser.Screenshot(
                TestContext.CurrentContext.Test.ClassName,
                TestContext.CurrentContext.Test.Name);
        }

        var tracePAth = Path.Combine(
            "playwright-traces",
            $"{TestContext.CurrentContext.Test.ClassName}",
            $"{TestContext.CurrentContext.Test.Name}.zip");
        await Browser.StopTracing(tracePAth);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Browser.Page!.CloseAsync();
        await Browser.Context!.CloseAsync();
    }
}
using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class LinksPageTests
{
    private LinksPage Page { get; set; }
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
            .OpenNewPage<LinksPage>();
        Browser.AddRequestResponseLogger();
        await Page!.Open();
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
    public async Task CheckCreatedApi()
    {
        await Page!.CreatedLink.ClickAsync();

        var linkResponse = await Page!.ResultText.InnerTextAsync();
        Assert.That(linkResponse, Is.EqualTo("Link has responded with staus 201 and status text Created"), "Main page should contain the expected text");
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
using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class BrokenLinksAndImagesPageTests
{
    private BrokenLinksAndImagesPage Page { get; set; }
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
            .OpenNewPage<BrokenLinksAndImagesPage>();
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
    public async Task CheckImages()
    {
        var validImage = await Page!.CheckImageValidity("Toolsqa.jpg");
        await Browser.Page!.GoBackAsync();
        var brokenImage = await Page!.CheckImageValidity("Toolsqa_1.jpg");
        await Browser.Page!.GoBackAsync();

        Assert.Multiple(() =>
        {
            Assert.That(validImage, Is.True);
            Assert.That(brokenImage, Is.False);
        });
    }


    [Test]
    public async Task CheckValidLink()
    {
        await Page!.ValidLink.ClickAsync();
        var elementsText = await Page!.Elements.InnerTextAsync();
        await Browser.Page!.GoBackAsync();

        Assert.That(elementsText, Is.EqualTo("Elements"), "Main page should contain the expected text");
    }

    [Test]
    public async Task CheckBrokenLink()
    {
        await Page!.BrokenLink.ClickAsync();
        var siteText = await Page!.BrokenSiteText.InnerTextAsync();
        await Browser.Page!.GoBackAsync();

        Assert.That(siteText, Is.AtLeast("This page returned a 500 status code."), "Main page should contain the expected text");
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
using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class DynamicPropertiesPageTests
{
    private DynamicPropertiesPage Page { get; set; }
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
            .OpenNewPage<DynamicPropertiesPage>();
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
    public async Task CheckEnableButton()
    {
        await Page!.Page.ReloadAsync();
        var isEnabledInitially = await Page!.EnableButton.IsEnabledAsync();
        Assert.That(isEnabledInitially, Is.False, "The button should be initially disabled.");

        await Task.Delay(5000);

        var isEnabledAfterDelay = await Page!.EnableButton.IsEnabledAsync();
        Assert.That(isEnabledAfterDelay, Is.True, "The button should be enabled after 5 seconds.");
    }

    [Test]
    public async Task CheckColorChaneButton()
    {
        var button = Page!.ColorButton;
        var colorInitially = await Page!.CheckColor(button);
        Assert.That(colorInitially, Is.EqualTo("rgb(255, 255, 255)"), "The button text should be initially white.");

        await Task.Delay(5000);

        var colorAfterDelay = await Page!.CheckColor(button);
        Assert.That(colorAfterDelay, Is.EqualTo("rgb(220, 53, 69)"), "The button text should be red after 5 seconds.");
    }

    [Test]
    public async Task CheckAppearingButton()
    {
        var visibilityInitially = await Page!.EnableButton.IsVisibleAsync();
        Assert.That(visibilityInitially, Is.False, "The button should be initially invisible.");

        await Task.Delay(5000);

        var visibilityAfterDelay = await Page!.EnableButton.IsVisibleAsync();
        Assert.That(visibilityAfterDelay, Is.True, "The button should be visible after 5 seconds.");
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
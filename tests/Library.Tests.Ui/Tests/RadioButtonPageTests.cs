using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class RadioButtonPageTests
{
    private RadioButtonPage Page { get; set; }
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
            .OpenNewPage<RadioButtonPage>();
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
    public async Task OpenRadioBoxPage_TitleIsCorrect()
    {
        var isTitleCorrect = await Page!.VerifyTitleAsync();

        Assert.That(isTitleCorrect, Is.True, "The page title is not correct.");
    }

    [Test]
    public async Task CheckYesRadioOutput()
    {
        await Page!.ClickYesRadioAsync();

        var output = await Page.GetOutputTextAsync();

        Assert.Multiple(() =>
        {
            Assert.That(output, Is.EqualTo("You have selected Yes"), "Output is incorrect.");
        });
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
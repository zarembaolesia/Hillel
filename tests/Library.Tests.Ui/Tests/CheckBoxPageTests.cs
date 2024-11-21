using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class CheckBoxPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUp = new();
    private CheckBoxPage? Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUp
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithSlowMo(100)
            .WithTimeout(10000)
            .WithViewportSize(1900, 1080)
            .WithVideoSize(1900, 1080)
            .SaveVideo("videos/")
            .WithArgs("--start-maximized")
            .OpenNewPage<CheckBoxPage>();
        _browserSetUp.AddRequestResponseLogger();
        await Page!.Open();
    }

    [SetUp]
    public async Task SetUp()
    {
        var traceName = TestContext.CurrentContext.Test.ClassName + "/" + TestContext.CurrentContext.Test.Name;
        await _browserSetUp.StartTracing(traceName);
    }

    [Test]
    public async Task NavigateToCheckBox_CheckThePageIsCorrect()
    {
        var pageTitle = await Page!.VerifyCheckBoxTitleAsync();
        var homeBox = await Page.CheckHomeBoxTitleAsync();

        Assert.True(pageTitle, "The Check Box page title is incorrect.");
        Assert.True(homeBox, "The Home check box title is not visible.");
    }

    [Test]
    public async Task NavigateToCheckBox_WhenIUnrollHome_ElementShouldBeDisplayed()
    {
        await Page!.ClickExpandAllAsync();
        var unrolledFolders = await Page.CheckUnrolledFoldersAsync();

        await Page.ClickCollapseAllAsync();
        var foldersCollapsed = !(await Page.CheckUnrolledFoldersAsync());

        Assert.True(unrolledFolders, "The folders were not unrolled as expected.");
        Assert.True(foldersCollapsed, "The folders were not collapsed as expected.");
    }

    [Test]
    public async Task CheckBox_WhenChecked_ShouldDisplayCheckedIcon()
    {
        await Page!.ClickExpandAllAsync();
        await Page.ClickHomeCheckBoxAsync();

        var resultText = await Page.GetResultTextAsync();

        var areAllChecked = await Page.AreAllCheckBoxesCheckedAsync();

        var expectedSelections = new[]
        {
        "home", "desktop", "notes", "commands", "documents", "workspace",
        "react", "angular", "veu", "office", "public", "private",
        "classified", "general", "downloads", "wordFile", "excelFile"
    };

        Assert.True(areAllChecked, "Not all checkboxes were checked.");
        Assert.True(resultText.Contains("You have selected :"),
            "The result text does not start with the expected prefix.");

        foreach (var selection in expectedSelections)
        {
            Assert.True(resultText.Contains(selection, StringComparison.OrdinalIgnoreCase),
                $"The result text does not contain '{selection}'.");
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            await _browserSetUp.Screenshot(
                TestContext.CurrentContext.Test.ClassName,
                TestContext.CurrentContext.Test.Name);
        }

        var tracePAth = Path.Combine(
            "playwright-traces",
            $"{TestContext.CurrentContext.Test.ClassName}",
            $"{TestContext.CurrentContext.Test.Name}.zip");
        await _browserSetUp.StopTracing(tracePAth);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUp.Page!.CloseAsync();
        await _browserSetUp.Context!.CloseAsync();
    }
}
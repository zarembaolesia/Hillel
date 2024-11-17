using Library.Test.Utils.Tests.Ui.Fixtures;
using Library.Test.Utils.Tests.Ui.PageObjects;
using NUnit.Framework.Interfaces;

namespace Library.Tests.Ui.Tests;

[TestFixture]
public class WebTablePageTests
{
    private WebTablePage Page { get; set; }
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
            .OpenNewPage<WebTablePage>();
        Browser.AddRequestResponseLogger();
        await Page!.OpenAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        var traceName = TestContext.CurrentContext.Test.ClassName + "/" + TestContext.CurrentContext.Test.Name;
        await Browser.StartTracing(traceName);
    }

    [Test]
    public async Task FirstWebTableTest()
    {
        var title = await Page!.Title?.InnerTextAsync();
        var filledRowsCount = await Page!.FindFilledRowsAsync();
        var columns = await Page!.TableColumns.CountAsync();
        var rowOne = await Page!.GetRowValuesAsync(1);
        var rowTwo = await Page!.GetRowValuesAsync(2);
        var rowThree = await Page!.GetRowValuesAsync(3);

        Assert.Multiple(() =>
        {
            Assert.That(title, Is.EqualTo("Web Tables"));
            Assert.That(filledRowsCount.Count, Is.EqualTo(3));
            Assert.That(columns, Is.EqualTo(7));
            Assert.That(rowOne[0], Is.EqualTo("Cierra"));
            Assert.That(rowTwo[0], Is.EqualTo("Alden"));
            Assert.That(rowThree[0], Is.EqualTo("Kierra"));
        });
    }

    [Test]
    public async Task WebTables_SortByAge()
    {
        await Page!.SortByAgeAsync();

        var rowOne = await Page!.GetRowValuesAsync(1);
        var rowTwo = await Page!.GetRowValuesAsync(2);
        var rowThree = await Page!.GetRowValuesAsync(3);

        Assert.Multiple(() =>
        {
            Assert.That(rowOne[0], Is.EqualTo("Kierra"));
            Assert.That(rowTwo[0], Is.EqualTo("Cierra"));
            Assert.That(rowThree[0], Is.EqualTo("Alden"));
            Assert.That(rowOne[2], Is.EqualTo("29"));
            Assert.That(rowTwo[2], Is.EqualTo("39"));
            Assert.That(rowThree[2], Is.EqualTo("45"));
        });
    }

    [Test]
    public async Task WebTables_AddAndDeleteRecord()
    {
        await Page!.AddButton.ClickAsync();

        await Page!.SetFirstNameAsync("Olesia");
        await Page!.SetLastNameAsync("Zaremba");
        await Page!.SetEmailAsync("zaremba_olesia@email.com");
        await Page!.SetAgeAsync(25);
        await Page!.SetSalaryAsync(1500000);
        await Page!.SetDepartmentAsync("Product Development");

        await Page!.SubmitAsync();

        var filledRowsCount = await Page!.FindFilledRowsAsync();
        var rowFour = await Page!.GetRowValuesAsync(4);

        Assert.Multiple(() =>
        {
            Assert.That(filledRowsCount?.Count, Is.EqualTo(4));
            Assert.That(rowFour[0], Is.EqualTo("Olesia"));
            Assert.That(rowFour[1], Is.EqualTo("Zaremba"));
            Assert.That(rowFour[2], Is.EqualTo("25"));
            Assert.That(rowFour[3], Is.EqualTo("zaremba_olesia@email.com"));
            Assert.That(rowFour[4], Is.EqualTo("1500000"));
            Assert.That(rowFour[5], Is.EqualTo("Product Development"));
        });

        await Page!.DeleteRowAsync(4);

        filledRowsCount = await Page!.FindFilledRowsAsync();
        Assert.That(filledRowsCount?.Count, Is.EqualTo(3));
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
using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public class CheckBoxPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/checkbox";
    public string ExpectedTitle { get; } = "Check Box";

    private ILocator CheckBoxTitle => Page!.Locator("//h1[contains(text(),'Check Box')]");
    private ILocator HomeBoxTitle => Page!.Locator("//span[@class='rct-title' and text()='Home']");
    private ILocator ExpandAllButton => Page!.Locator("//button[@aria-label='Expand all' and @title='Expand all']");
    private ILocator CollapseAllButton => Page!.Locator("//button[@aria-label='Collapse all' and @title='Collapse all']");
    private ILocator DesktopFolder => Page!.Locator("//span[@class='rct-title' and text()='Desktop']");
    private ILocator DocumentsFolder => Page!.Locator("//span[@class='rct-title' and text()='Documents']");
    private ILocator DownloadsFolder => Page!.Locator("//span[@class='rct-title' and text()='Downloads']");
    private ILocator HomeCheckBox => Page!.Locator("//label[@for='tree-node-home']");
    private ILocator NotesCheckBox => Page!.Locator("//label[@for='tree-node-notes']");
    private ILocator DesktopCheckBox => Page!.Locator("//label[@for='tree-node-desktop']");
    private ILocator DocumentsCheckBox => Page!.Locator("//label[@for='tree-node-documents']");
    private ILocator DownloadsCheckBox => Page!.Locator("//label[@for='tree-node-downloads']");
    private ILocator Result => Page!.Locator("//*[@id='result']");

    public async Task<CheckBoxPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }

    public async Task NavigateToPageAsync()
    {
        await Page!.GotoAsync("https://demoqa.com/checkbox");
    }

    public async Task<bool> VerifyCheckBoxTitleAsync()
    {
        return await CheckBoxTitle.IsVisibleAsync();
    }

    public async Task<bool> CheckHomeBoxTitleAsync()
    {
        return await HomeBoxTitle.IsVisibleAsync();
    }

    public async Task<string> GetResultTextAsync()
    {
        return await Result.TextContentAsync() ?? string.Empty;
    }

    public async Task<bool> CheckUnrolledFoldersAsync()
    {
        var folderLocators = new[] { DesktopFolder, DocumentsFolder, DownloadsFolder };
        var visibilityChecks = folderLocators.Select(locator => locator.IsVisibleAsync());
        var results = await Task.WhenAll(visibilityChecks);

        return results.All(visible => visible);
    }

    public async Task ClickExpandAllAsync()
    {
        await ExpandAllButton.ClickAsync();
    }

    public async Task ClickCollapseAllAsync()
    {
        await CollapseAllButton.ClickAsync();
    }

    public async Task ClickHomeCheckBoxAsync()
    {
        await HomeCheckBox.ClickAsync();
    }

    public async Task ClickNotesCheckBoxAsync()
    {
        await NotesCheckBox.ClickAsync();
    }

    public async Task<bool> AreAllCheckBoxesCheckedAsync()
    {
        var checkBoxLocators = new[] { HomeCheckBox, DesktopCheckBox, DocumentsCheckBox, DownloadsCheckBox };
        var checkStates = checkBoxLocators.Select(locator => locator.IsCheckedAsync());
        var results = await Task.WhenAll(checkStates);

        return results.All(isChecked => isChecked);
    }
}
using Microsoft.Playwright;

namespace Library.Test.Utils.Tests.Ui.PageObjects;

public partial class WebTablePage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/webtables";
    public string ExpectedTitle { get; } = "Web Tables";

    public ILocator Title => Page!.Locator("xpath=//h1[contains(text(),'Web Tables')]");
    public ILocator AgeHeader => Page!.Locator("//div[@class='rt-resizable-header-content' and text()='Age']");
    public ILocator AddButton => Page!.Locator("//button[@id='addNewRecordButton']");

    public ILocator DeleteButton(int rowNumber) => Page!.Locator($"//span[@id='delete-record-{rowNumber}']");

    public ILocator FirstNameInput => Page!.Locator("//div[contains(@class, 'modal-content')]//input[@id='firstName']");
    public ILocator LastNameInput => Page!.Locator("//div[contains(@class, 'modal-content')]//input[@id='lastName']");
    public ILocator AgeInput => Page!.Locator("//div[contains(@class, 'modal-content')]//input[@id='age']");
    public ILocator EmailInput => Page!.Locator("//div[contains(@class, 'modal-content')]//input[@id='userEmail']");
    public ILocator SalaryInput => Page!.Locator("//div[contains(@class, 'modal-content')]//input[@id='salary']");
    public ILocator DepartmentInput => Page!.Locator("//div[contains(@class, 'modal-content')]//input[@id='department']");
    public ILocator SubmitButton => Page!.Locator("//div[contains(@class, 'modal-content')]//button[@id='submit']");
    public ILocator TableRows => Page!.Locator("//div[@class='rt-tr-group' and .//div[@role='gridcell' and normalize-space(.) != '']]");
    public ILocator TableColumns => Page!.Locator("//div[@class='rt-thead -header']/div[@class='rt-tr']/div");

    public async Task<WebTablePage> OpenAsync()
    {
        await Page!.GotoAsync(Url);
        return this;
    }

    public async Task<List<string[]>> FindFilledRowsAsync()
    {
        var rows = await TableRows.AllAsync();
        var filledRows = new List<string[]>();

        foreach (var row in rows)
        {
            var cellValues = await row.Locator("div[role='gridcell']").AllTextContentsAsync();

            if (cellValues.Any(cell => !string.IsNullOrWhiteSpace(cell)))
            {
                filledRows.Add(cellValues.ToArray());
            }
        }

        return filledRows;
    }

    public async Task<string[]> GetRowValuesAsync(int rowNumber)
    {
        var rowCells = Page!.Locator($"//div[@class='rt-tr-group'][{rowNumber}]//div[@role='gridcell']");

        var values = (await rowCells.AllTextContentsAsync()).ToArray();

        return values;
    }

    public async Task<WebTablePage> SortByAgeAsync()
    {
        await AgeHeader.ClickAsync();
        return this;
    }

    public async Task<WebTablePage> AddNewRecordAsync()
    {
        await AddButton.ClickAsync();
        return this;
    }

    public async Task<WebTablePage> SetFirstNameAsync(string name)
    {
        await FirstNameInput.FillAsync(name);
        return this;
    }

    public async Task<WebTablePage> SetLastNameAsync(string name)
    {
        await LastNameInput.FillAsync(name);
        return this;
    }

    public async Task<WebTablePage> SetEmailAsync(string email)
    {
        await EmailInput.FillAsync(email);
        return this;
    }

    public async Task<WebTablePage> SetAgeAsync(int age)
    {
        await AgeInput.FillAsync(age.ToString());
        return this;
    }

    public async Task<WebTablePage> SetSalaryAsync(int salary)
    {
        await SalaryInput.FillAsync(salary.ToString());
        return this;
    }

    public async Task<WebTablePage> SetDepartmentAsync(string department)
    {
        await DepartmentInput.FillAsync(department);
        return this;
    }

    public async Task<WebTablePage> SubmitAsync()
    {
        await SubmitButton.ClickAsync();
        return this;
    }

    public async Task<WebTablePage> DeleteRowAsync(int row)
    {
        var deleteButton = DeleteButton(row);
        await deleteButton.ClickAsync();
        return this;
    }
}

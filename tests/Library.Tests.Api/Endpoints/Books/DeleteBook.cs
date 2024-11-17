using System.Net;
using Library.Contracts.Domain;
using Library.Tests.Api.TestFixtures;

namespace Library.Tests.Api.Endpoints.Books;

[TestFixture]
public class DeleteBook : GlobalSetUp
{
    [SetUp]
    public async Task SetUp()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
    }

    [Test]
    public async Task DeleteBook_WhenBookExists_ReturnOk()
    {
        var newBook = new Book()
        {
            Title = "ToDelete",
            Author = "None"
        };

        var bookCreated = await LibraryHttpService.PostBook(newBook);
        HttpResponseMessage response = await LibraryHttpService.DeleteBook(newBook.Title, newBook.Author);
        var jsonString = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(jsonString.Contains("ToDelete by None deleted"));
        });
    }
}
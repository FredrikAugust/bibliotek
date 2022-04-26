using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Books.Queries.GetAllBooks;
using Application.IntegrationTests.Common;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Books.Queries;

[Collection("DbCollection")]
public class GetAllTests : TestsWithHelper
{
    [Fact]
    public async Task GetAllTests_ShouldReturnAll_WhenEmpty()
    {
        var result = await Mediator.Send(new GetAllBooksQuery());

        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllTests_ShouldReturnAll()
    {

        ApplicationDbContext.Books.AddRange(new List<Book>
        {
            new()
            {
                Name = "Superbook 42",
                Isbn = new Isbn("test"),
            },
            new()
            {
                Name = "My cool cats",
                Isbn = new Isbn("ok")
            }
        });

        await ApplicationDbContext.SaveAsync();

        var result = await Mediator.Send(new GetAllBooksQuery());

        result.Books.Count().Should().Be(2);
    }
}
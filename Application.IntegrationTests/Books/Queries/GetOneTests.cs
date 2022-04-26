using System;
using System.Threading.Tasks;
using Application.Books.Queries.GetBook;
using Application.IntegrationTests.Common;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Books.Queries;

[Collection("DbCollection")]
public class GetOneTests : TestsWithHelper
{
    [Fact]
    public async Task GetOneTests_ShouldNotReturn_IfNotExist()
    {
        var result = await Mediator.Send(new GetBookQuery {BookId = Guid.NewGuid()});

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetOneTests_ShouldReturn_IfExist()
    {
        var addedBook = ApplicationDbContext.Books.Add(new Book
        {
            Name = "Super book 42",
            Isbn = new Isbn("test"),
        });

        await ApplicationDbContext.SaveAsync();

        var result = await Mediator.Send(new GetBookQuery {BookId = addedBook.Entity.Id});

        result.Should().NotBeNull();
    }
}
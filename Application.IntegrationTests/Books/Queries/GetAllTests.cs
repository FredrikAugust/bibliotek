using System;
using System.Collections.Generic;
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
        var book1Name = Guid.NewGuid().ToString();
        var book2Name = Guid.NewGuid().ToString();
        
        ApplicationDbContext.Books.AddRange(new List<Book>
        {
            new()
            {
                Name = book1Name,
                Isbn = new Isbn("test"),
            },
            new()
            {
                Name = book2Name,
                Isbn = new Isbn("ok")
            }
        });

        await ApplicationDbContext.SaveAsync();

        var result = await Mediator.Send(new GetAllBooksQuery());

        result.Books.Should().Contain(dto => dto.Name == book1Name).And.Contain(dto => dto.Name == book2Name);
    }
}
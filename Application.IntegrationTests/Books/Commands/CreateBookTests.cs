using System;
using System.Threading.Tasks;
using Application.Books.Commands.CreateBook;
using Application.IntegrationTests.Common;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Books.Commands;

[Collection("DbCollection")]
public class CreateBookTests : TestsWithHelper
{
    [Fact]
    public async Task CreateBookTests_CreateBook_CreatesBook()
    {
        var book = new Book
        {
            Name = "test book creation",
            Isbn = new Isbn("test")
        };
        
        var result = await Mediator.Send(new CreateBookCommand
        {
            Name = book.Name,
            IsbnRaw = book.Isbn.RawValue
        });


        result?.Id.Should().NotBeNull();

        var dbEntry = await ApplicationDbContext.Books.FindAsync(Guid.Parse(result!.Id!));

        dbEntry.Should().NotBeNull();
        dbEntry!.Name.Should().Be(book.Name);
    }
}
using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using Application.Rentals.Commands.CreateRental;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Rentals.Commands;

[Collection("DbCollection")]
public class CreateRentalTests : TestsWithHelper
{
    [Fact]
    public async Task CreateRentalTests_ShouldBeAbleToRent_IfBookExists()
    {
        var book = new Book
        {
            Name = "test book",
            Isbn = new Isbn("test isbn")
        };

        var bookEntity = ApplicationDbContext.Books.Add(book);
        await ApplicationDbContext.SaveAsync();

        var result = await Mediator.Send(new CreateRentalCommand
        {
            BookId = bookEntity.Entity.Id
        });

        result.Should().NotBeNull();

        bookEntity.Entity.Rentals.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task CreateRentalTests_ShouldNotBeAbleToRent_IfBookNotExists()
    {
        var result = await Mediator.Send(new CreateRentalCommand
        {
            BookId = Guid.NewGuid()
        });

        result.Should().BeNull();
    }
}
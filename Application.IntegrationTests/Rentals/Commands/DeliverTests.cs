using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using Application.Rentals.Commands.Deliver;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Rentals.Commands;

[Collection("DbCollection")]
public class DeliverTests : TestsWithHelper
{
    [Fact]
    public async Task DeliverTests_ShouldBeAbleToHandInBook()
    {
        var book = new Book
        {
            Name = "test",
            Isbn = new Isbn("test"),
            Rentals = new List<Rental>
            {
                new()
                {
                    Start = DateTime.Now,
                    UserId = TestHelper.UserId
                }
            }
        };

        var entity = ApplicationDbContext.Books.Add(book);
        await ApplicationDbContext.SaveAsync();

        var result = await Mediator.Send(new DeliverCommand
        {
            RentalId = entity.Entity.Rentals.First().Id
        });

        result.Should().BeTrue();

        entity.Entity.Rentals.First().End.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeliverTests_ShouldNotBeAbleToHandInBook_WhenNotExists()
    {
        var result = await Mediator.Send(new DeliverCommand
        {
            RentalId = Guid.NewGuid()
        });

        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task DeliverTests_ShouldNotBeAbleToHandInBook_WhenNotYours()
    {
        var book = new Book
        {
            Name = "test",
            Isbn = new Isbn("test"),
            Rentals = new List<Rental>
            {
                new()
                {
                    Start = DateTime.Now,
                    UserId = "4-8-15-16-23-42"
                }
            }
        };

        var entity = ApplicationDbContext.Books.Add(book);
        await ApplicationDbContext.SaveAsync();

        var result = await Mediator.Send(new DeliverCommand
        {
            RentalId = entity.Entity.Rentals.First().Id
        });

        result.Should().BeFalse();

        entity.Entity.Rentals.First().End.Should().BeNull();
    }
}
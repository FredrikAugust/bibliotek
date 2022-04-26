using Application.Common.Mappings;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Rentals.Queries.Active;

public class BookWithoutRentalsDto : IMapFrom<Book>
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public Isbn? Isbn { get; set; }
}
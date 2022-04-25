using Domain.ValueObjects;

namespace Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public Isbn? Isbn { get; set; }

    public IEnumerable<Rental> Rentals { get; set; } = null!;
}
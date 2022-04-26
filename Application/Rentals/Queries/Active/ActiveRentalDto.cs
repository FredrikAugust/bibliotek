using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Rentals.Queries.Active;

public class ActiveRentalDto : IMapFrom<Rental>
{
    public Guid Id { get; set; }

    public BookWithoutRentalsDto Book { get; set; } = null!;

    public DateTime Start { get; set; }
}
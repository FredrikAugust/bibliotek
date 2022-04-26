using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Rentals.Commands.CreateRental;

public class CreateRentalVm : IMapFrom<Rental>
{
    public Guid BookId { get; set; }
    
    public DateTime Start { get; set; }

    public string UserId { get; set; } = null!;
}
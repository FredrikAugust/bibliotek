using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Books.Queries.GetBook;

public class BriefRentalDto : IMapFrom<Rental>
{
    public bool Active { get; set; }
    
    public Guid Id { get; set; }
    
    public DateTime Start { get; set; }
    
    public DateTime? End { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Rental, BriefRentalDto>()
            .ForMember(dto => dto.Active,expression => expression.MapFrom(rental => rental.End == null));
    }
}
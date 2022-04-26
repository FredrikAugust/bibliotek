using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Books.Queries.GetAllBooks;

public class BriefBookDto : IMapFrom<Book>
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool Available { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Book, BriefBookDto>()
            .ForMember(dto => dto.Available,
                expression => expression.MapFrom(book => book.Rentals.All(rental => rental.End != null)));
    }
}
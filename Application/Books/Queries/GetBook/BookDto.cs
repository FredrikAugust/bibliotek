using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Books.Queries.GetBook;

public class BookDto : IMapFrom<Book>
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Isbn { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Book, BookDto>()
            .ForMember(dto => dto.Isbn,expression => expression.MapFrom(book => book.Isbn.RawValue));
    }
}
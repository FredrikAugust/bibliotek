using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Books.Queries.GetAllBooks;

public class BookDto : IMapFrom<Book>
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }

    public string? Isbn { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Book, BookDto>()
            .ForMember(dto => dto.Isbn,expression => expression.MapFrom(book => book.Isbn.RawValue));
    }
}
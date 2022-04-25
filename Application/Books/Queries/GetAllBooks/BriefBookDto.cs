using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Books.Queries.GetAllBooks;

public class BriefBookDto : IMapFrom<Book>
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }
}
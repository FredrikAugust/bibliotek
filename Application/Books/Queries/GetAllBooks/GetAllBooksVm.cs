namespace Application.Books.Queries.GetAllBooks;

public class GetAllBooksVm
{
    public IEnumerable<BriefBookDto> Books { get; set; } = null!;
}
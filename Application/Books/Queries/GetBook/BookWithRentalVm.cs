namespace Application.Books.Queries.GetBook;

public class BookWithRentalVm
{
    public BookDto Book { get; set; }
    
    public IEnumerable<BriefRentalDto> Rentals { get; set; }
}
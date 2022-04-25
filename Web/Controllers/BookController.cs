using Application.Books.Commands.CreateBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Common;

namespace Web.Controllers;

[Authorize]
public class BookController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetAllBooksVm>> Get()
    {
        return Ok(await Mediator.Send(new GetAllBooksQuery()));
    }

    [HttpGet("{bookId:guid}")]
    public async Task<ActionResult<BookWithRentalVm>> GetBook(Guid bookId)
    {
        var result = await Mediator.Send(new GetBookQuery {BookId = bookId});

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BriefBookDto>> Create([FromBody] CreateBookCommand createBookCommand)
    {
        return Ok(await Mediator.Send(createBookCommand));
    }
}
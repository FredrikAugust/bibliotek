using Application.Books.Commands.CreateBook;
using Application.Books.Queries.GetAllBooks;
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

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookCommand createBookCommand)
    {
        return Ok(await Mediator.Send(createBookCommand));
    }
}
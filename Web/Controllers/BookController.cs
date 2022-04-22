using Application.Books.Queries.GetAllBooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Common;

namespace Web.Controllers;

[Authorize]
public class BookController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> Get()
    {
        return Ok(await Mediator.Send(new GetAllBooksQuery()));
    }
}
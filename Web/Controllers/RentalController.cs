using Application.Rentals.Commands.CreateRental;
using Application.Rentals.Commands.Deliver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Common;

namespace Web.Controllers;

[Authorize]
public class RentalController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateRentalVm>> Create([FromBody] CreateRentalCommand createRentalCommand)
    {
        var rental = await Mediator.Send(createRentalCommand);

        if (rental == null)
        {
            return BadRequest();
        }

        return Ok(rental);
    }

    [HttpPost("deliver")]
    public async Task<ActionResult> Deliver([FromBody] DeliverCommand deliverCommand)
    {
        var result = await Mediator.Send(deliverCommand);

        if (result) return Ok();

        return BadRequest();
    }
}
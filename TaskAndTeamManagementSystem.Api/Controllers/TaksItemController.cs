using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.GetById;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.GetList;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaksItemController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemPayload payload)
    {
        var command = new CreateTaskItemCommand { Payload = payload };
        await _mediator.Send(command);
        return Created(string.Empty, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskItemPayload payload)
    {
        var command = new UpdateTaskItemCommand { Id = id, Payload = payload };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var query = new GetTaskItemByIdRequest { Id = id };
        var result = await _mediator.Send(query);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TaskItemPaginationQuery query)
    {
        var request = new GetTaskItemListRequest
        {
            Query= query
        };

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var command = new DeleteTaskItemCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}

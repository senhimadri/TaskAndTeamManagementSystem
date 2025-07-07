using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Api.Helpers;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.GetById;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.GetList;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
public class TaskItemController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemPayload payload)
    {
        var command = new CreateTaskItemCommand { Payload = payload };
        var response = await _mediator.Send(command);
        return response.Match(
            onSuccess: () => CreatedAtAction(nameof(GetById),new { id = response }, response ),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskItemPayload payload)
    {
        var command = new UpdateTaskItemCommand { Id = id, Payload = payload };
        var response = await _mediator.Send(command);
        return response.Match(
              onSuccess: () => NoContent(),
              onValidationFailure: validationErrors => ValidationProblem(validationErrors),
              onFailure: error => BadRequest(error)
            );
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
            Query = query
        };

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var command = new DeleteTaskItemCommand { Id = id };
        var response = await _mediator.Send(command);
        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }
}

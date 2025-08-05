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
using TaskAndTeamManagementSystem.Identity;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
public class TaskItemController(IMediator _mediator, IAuthorizationService  _authService) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemPayload payload)
    {
        var command = new CreateTaskItemCommand { Payload = payload };
        var response = await _mediator.Send(command);

        return response.Match(onSuccess: createdItem => CreatedAtAction(
                nameof(GetById),
                new { id = response.Value },
                createdItem
            ),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Employee")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskItemPayload payload)
    {
        var requirement = new OwnTaskRequirement();
        var authResult = await _authService.AuthorizeAsync(User, id, requirement);

        if (!authResult.Succeeded)
            return Forbid();

        var command = new UpdateTaskItemCommand { Id = id, Payload = payload };
        var response = await _mediator.Send(command);
        return response.Match(
              onSuccess: () => NoContent(),
              onValidationFailure: validationErrors => ValidationProblem(validationErrors),
              onFailure: error => BadRequest(error)
            );
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Employee")]
    public async Task<IActionResult> GetById(long id)
    {
        var requirement = new OwnTaskRequirement();
        var authResult = await _authService.AuthorizeAsync(User, id, requirement);

        if (!authResult.Succeeded)
            return Forbid();

        var query = new GetTaskItemByIdRequest { Id = id };
        var result = await _mediator.Send(query);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Authorize(Policy = "Employee")]
    public async Task<IActionResult> GetList([FromQuery] TaskItemPaginationQuery query)
    {
        GetTaskItemListRequest request = new GetTaskItemListRequest {   Query = query   };

        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Manager")]
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

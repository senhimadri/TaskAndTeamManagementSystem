using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Api.Helpers;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;
using TaskAndTeamManagementSystem.Application.Features.Teams.Create;
using TaskAndTeamManagementSystem.Application.Features.Teams.Delete;
using TaskAndTeamManagementSystem.Application.Features.Teams.GetById;
using TaskAndTeamManagementSystem.Application.Features.Teams.GetList;
using TaskAndTeamManagementSystem.Application.Features.Teams.Update;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
public class TeamController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamPayload payload)
    {
        var command = new CreateTeamCommand { Payload = payload };
        var response = await _mediator.Send(command);

        return response.Match(
            onSuccess: createdId => CreatedAtAction(nameof(GetById), new { id = response.Value }, createdId),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeamPayload payload)
    {
        var command = new UpdateTeamCommand { Id = id, Payload = payload };
        var response = await _mediator.Send(command);
        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetTeamByIdRequest { Id = id };
        var result = await _mediator.Send(query);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] TeamPaginationQuery query)
    {
        var request = new GetTeamListRequest { Query = query };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteTeamCommand { Id = id };
        var response = await _mediator.Send(command);
        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }
}
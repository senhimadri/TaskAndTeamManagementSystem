using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Application.Features.Users.Create;
using TaskAndTeamManagementSystem.Application.Features.Users.Update;
using TaskAndTeamManagementSystem.Application.Features.Users.Delete;
using TaskAndTeamManagementSystem.Application.Features.Users.GetById;
using TaskAndTeamManagementSystem.Application.Features.Users.GetList;
using TaskAndTeamManagementSystem.Application.Features.Users.UpdatePassword;
using TaskAndTeamManagementSystem.Api.Helpers;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
public class UserController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserPayload payload)
    {
        var command = new CreateUserCommand { Payload = payload };
        var response = await _mediator.Send(command);

        return response.Match(
            onSuccess: userId => CreatedAtAction(
                nameof(GetById),
                new { id = userId },
                userId
            ),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserPayload payload)
    {
        var command = new UpdateUserCommand { Id = id, Payload = payload };
        var response = await _mediator.Send(command);

        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpPut("{id}/password")]
    public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdateUserPasswordPayload payload)
    {
        var command = new UpdateUserPasswordCommand { UserId = id, Payload = payload };
        var response = await _mediator.Send(command);

        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetUserByIdRequest { Id = id };
        var result = await _mediator.Send(query);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] UserPaginationQuery query)
    {
        var request = new GetUserListRequest { Query = query };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteUserCommand { Id = id };
        var response = await _mediator.Send(command);

        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }
}
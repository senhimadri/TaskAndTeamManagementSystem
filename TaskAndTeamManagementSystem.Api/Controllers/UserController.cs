using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Api.Helpers;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Application.Features.Users.Create;
using TaskAndTeamManagementSystem.Application.Features.Users.Delete;
using TaskAndTeamManagementSystem.Application.Features.Users.GetById;
using TaskAndTeamManagementSystem.Application.Features.Users.GetList;
using TaskAndTeamManagementSystem.Application.Features.Users.Update;
using TaskAndTeamManagementSystem.Application.Features.Users.UpdatePassword;
using TaskAndTeamManagementSystem.Identity;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(500)]
public class UserController(IMediator _mediator, IAuthorizationService _authService) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
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
    [Authorize(Policy = "Admin")]
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
    [Authorize(Policy = "Employee")]
    public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdateUserPasswordPayload payload)
    {
        var requirement = new OwnUserRequirement();
        var authResult = await _authService.AuthorizeAsync(User, id, requirement);

        if (!authResult.Succeeded)
            return Forbid();

        var command = new UpdateUserPasswordCommand { UserId = id, Payload = payload };
        var response = await _mediator.Send(command);

        return response.Match(
            onSuccess: () => NoContent(),
            onValidationFailure: validationErrors => ValidationProblem(validationErrors),
            onFailure: error => BadRequest(error)
        );
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Employee")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var requirement = new OwnUserRequirement();
        var authResult = await _authService.AuthorizeAsync(User, id, requirement);

        if (!authResult.Succeeded)
            return Forbid();

        var query = new GetUserByIdRequest { Id = id };
        var result = await _mediator.Send(query);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    [Authorize(Policy = "Manager")]
    public async Task<IActionResult> GetList([FromQuery] UserPaginationQuery query)
    {
        var request = new GetUserListRequest { Query = query };
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
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
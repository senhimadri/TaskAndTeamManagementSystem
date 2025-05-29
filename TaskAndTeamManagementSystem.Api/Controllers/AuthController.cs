using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Application.Dtos.LoginDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Features.Auths.Login;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginPayload payload)
    {
        var command = new LoginCommand { Payload = payload };
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}

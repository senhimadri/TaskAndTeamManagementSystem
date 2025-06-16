using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;
using TaskAndTeamManagementSystem.Application.Features.Auths.GoogleLogin;
using TaskAndTeamManagementSystem.Application.Features.Auths.Login;

namespace TaskAndTeamManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginPayload payload)
    {
        var command = new LoginCommand { Payload = payload };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin( string token)
    {
        var command = new GoogleLoginCommand { Token = token };
        var loginResponse = await _mediator.Send(command);
        return Ok(loginResponse);
    }
}

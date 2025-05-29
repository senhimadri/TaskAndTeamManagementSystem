using System.ComponentModel.DataAnnotations;

namespace TaskAndTeamManagementSystem.Application.Dtos.LoginDtos;

public record LoginPayload([Required]string UserName, [Required][DataType(DataType.Password)] string Password);

public record LoginResponse(string AccessToken, string RefreshToken);

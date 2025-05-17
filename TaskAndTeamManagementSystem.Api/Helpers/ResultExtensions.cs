using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Application.Helpers.Results;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Api.Helpers;

public static class ResultExtensions
{
    public static IActionResult Match(this Result result,
        Func<IActionResult> onSuccess,
        Func<ValidationProblemDetails, IActionResult> onValidationFailure,
        Func<Error, IActionResult> onFailure)
    {
        return result.IsSuccess ? onSuccess()
            : (result.IsValidationFailure && result.ValidationErrors is not null) ? onValidationFailure(result.ValidationErrors.ToValidationDetails())
            : onFailure(result.Error);
    }

    public static ValidationProblemDetails ToValidationDetails(this Dictionary<string, string[]> ValidationErrors)
    {
        return new ValidationProblemDetails(ValidationErrors)
        {
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest
        };

    }
}

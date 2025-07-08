using Microsoft.AspNetCore.Mvc;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Api.Helpers;

public static class ResultExtensions
{
    public static IActionResult Match(this Result result,
        Func<IActionResult> onSuccess,
        Func<ValidationProblemDetails, IActionResult> onValidationFailure,
        Func<Error, IActionResult> onFailure)
    {
        if (result.IsSuccess)
        {
            return onSuccess();
        }

        if (result.IsValidationFailure && result.ValidationErrors is not null)
        {
            return onValidationFailure(result.ValidationErrors.ToValidationDetails());
        }

        return onFailure(result.Error);
    }

    public static IActionResult Match<T>(this Result<T> result,
            Func< T? ,IActionResult> onSuccess,
            Func<ValidationProblemDetails, IActionResult> onValidationFailure,
            Func<Error, IActionResult> onFailure)
    {
        if (result.IsSuccess)
        {
            return onSuccess(result.Value);
        }

        if (result.IsValidationFailure && result.ValidationErrors is not null)
        {
            return onValidationFailure(result.ValidationErrors.ToValidationDetails());
        }

        return onFailure(result.Error);
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

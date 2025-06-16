using FluentValidation;
using FluentValidation.Results;

namespace TaskAndTeamManagementSystem.Application.Extensions;

public static class ValidationExtensions
{
    public static Dictionary<string, string[]> ToValidationErrorList(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
                   .GroupBy(e => e.PropertyName)
                   .ToDictionary(
                       group => group.Key,
                       group => group.Select(e => e.ErrorMessage).ToArray());

        return errors;
    }
}

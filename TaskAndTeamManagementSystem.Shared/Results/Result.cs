namespace TaskAndTeamManagementSystem.Shared.Results;

public sealed class Result
{
    private Result(bool isSuccess, Error error, Dictionary<string, string[]>? validationErrors = null)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            throw new ArgumentException("Invalid result state: success/failure mismatch with error.");

        IsSuccess = isSuccess;
        Error = error;

        ValidationErrors = validationErrors;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public Dictionary<string, string[]>? ValidationErrors { get; }
    public bool IsValidationFailure => ValidationErrors?.Any() == true;
    public static Result Success() => new(true, Error.None);
    private static Result Failure(Error error) => new(false, error);
    private static Result ValidationFailure(Dictionary<string, string[]> errors) => new(false, Errors.ValidationFailed, errors);

    public static implicit operator Result(Error error) => Failure(error);
    public static implicit operator Result(Dictionary<string, string[]> errors) => ValidationFailure(errors);
}

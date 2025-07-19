namespace TaskAndTeamManagementSystem.Shared.Results;

public class MasterResult
{
    protected MasterResult(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            throw new ArgumentException("Invalid result state: success/failure mismatch with error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public MasterResult(bool isSuccess, Error error, Dictionary<string, string[]> validationErrors)
        : this(isSuccess, error)
    {
        ValidationErrors = validationErrors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; } = Error.None;

    public Dictionary<string, string[]>? ValidationErrors { get; }
    public bool IsValidationFailure => ValidationErrors?.Any() == true;
}

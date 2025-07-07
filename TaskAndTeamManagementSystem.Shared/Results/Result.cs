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


public sealed class Result<T> : MasterResult 
{
    private Result(bool isSuccess, Error error, T? value) : base(isSuccess, error )
    {

        Value = value;
    }

    private Result(bool isSuccess, Error error, T? value, Dictionary<string, string[]> validationErrors)
                         :  base(isSuccess, error, validationErrors)
    {
        Value = value;
    }

    public T? Value { get; }
    public static Result<T> Success(T value) => new(true, Error.None, value);
    private static Result<T> Failure(Error error) => new(false, error, default);
    private static Result<T> ValidationFailure(Dictionary<string, string[]> errors) => new(false, Errors.ValidationFailed, default, errors);
   
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
    public static implicit operator Result<T>(Dictionary<string, string[]> errors) => ValidationFailure(errors);
}



public sealed class Result : MasterResult
{
    private Result(bool isSuccess, Error error)  : base(isSuccess, error)
    {

    }

    private Result(bool isSuccess, Error error, Dictionary<string, string[]> validationErrors)
        : base(isSuccess, error, validationErrors)
    {

    }


    public static Result Success() => new(true, Error.None);
    private static Result Failure(Error error) => new(false, error);
    private static Result ValidationFailure(Dictionary<string, string[]> errors) => new(false, Errors.ValidationFailed, errors);

    public static implicit operator Result(Error error) => Failure(error);
    public static implicit operator Result(Dictionary<string, string[]> errors) => ValidationFailure(errors);
}

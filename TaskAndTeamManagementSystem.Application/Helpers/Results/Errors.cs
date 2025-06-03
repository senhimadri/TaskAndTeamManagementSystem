namespace TaskAndTeamManagementSystem.Application.Helpers.Results;

public static class Errors
{
    public static readonly Error TaskNotFound = new Error(404, "Task not found");

    public static readonly Error UserNotAuthenticated = new Error(401, "User is not authenticated.");

    public static readonly Error BadRequest = new Error(400, "Bad request.");
    public static readonly Error UnauthorizedAccess = new Error(401, "Unauthorized access.");
    public static readonly Error Forbidden = new Error(403, "Forbidden access.");
    public static readonly Error ValidationFailed = new Error(422, "Validation failed.");
    public static readonly Error Conflict = new Error(409, "Conflict occurred.");
    public static readonly Error PreconditionFailed = new Error(412, "Precondition failed.");
    public static readonly Error NotAcceptable = new Error(406, "Not acceptable.");
    public static readonly Error UnsupportedMediaType = new Error(415, "Unsupported media type.");

    public static Error NewError(int code, string message) => new Error(code, message);
}

namespace TaskAndTeamManagementSystem.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                if (_env.IsDevelopment())
                {
                    var devResponse = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        ExceptionType = ex.GetType().Name
                    };
                    await context.Response.WriteAsJsonAsync(devResponse);
                }
                else
                {
                    var prodResponse = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "An unexpected error occurred. Please try again later."
                    };
                    await context.Response.WriteAsJsonAsync(prodResponse);
                }
            }
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}

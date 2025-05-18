using Serilog;

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

                var statusCode = StatusCodes.Status500InternalServerError;
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                Log.Error(ex, "❌ Exception at {Path}", context.Request.Path);

                if (_env.IsDevelopment())
                {
                    var devResponse = new
                    {
                        StatusCode = statusCode,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Source = ex.Source,
                        ExceptionType = ex.GetType().Name
                    };

                    Log.Information("⬅️ Response: {@Response}", devResponse);
                    await context.Response.WriteAsJsonAsync(devResponse);
                }
                else
                {
                    var prodResponse = new
                    {
                        StatusCode = statusCode,
                        Message = "An unexpected error occurred. Please try again later."
                    };

                    Log.Information("⬅️ Response: {@Response}", prodResponse);
                    await context.Response.WriteAsJsonAsync(prodResponse);
                }
            }
        }
    }
}

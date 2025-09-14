using System.Net;
using System.Text.Json;

namespace SmsApi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException argEx)
        {
            _logger.LogWarning(argEx, "Validation failed: {Message}", argEx.Message);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var problemDetails = new
            {
                error = argEx.Message,
                status = HttpStatusCode.BadRequest.ToString(),
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
        catch (BadHttpRequestException badReqEx)
        {
            _logger.LogWarning(badReqEx, "Bad request: {Message}", badReqEx.Message);

            context.Response.StatusCode = badReqEx.StatusCode;
            context.Response.ContentType = "application/json";

            var problemDetails = new
            {
                error = badReqEx.Message,
                status = ((HttpStatusCode)badReqEx.StatusCode).ToString(),
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var problemDetails = new
            {
                error = "An unexpected error occurred. Please try again later.",
                status = HttpStatusCode.InternalServerError.ToString(),
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}

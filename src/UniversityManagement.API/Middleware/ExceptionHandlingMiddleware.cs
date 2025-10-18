using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";

        if (exception is ValidationException validationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = validationException.Message;
        }
        else if (exception is UnauthorizedAccessException unauthorizedException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            message = string.IsNullOrWhiteSpace(unauthorizedException.Message)
                ? "Unauthorized"
                : unauthorizedException.Message;
        }
        else if (exception is KeyNotFoundException notFoundException)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            message = notFoundException.Message;
        }

        _logger.LogError(
            exception,
            "Unhandled exception processing {Method} {Path}. TraceId: {TraceId}. Responding with {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            context.TraceIdentifier,
            statusCode);

        var response = ApiResponse<object>.Fail(message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(payload);
    }
}

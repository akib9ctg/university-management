using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Net;
using System.Text.Json;
using UniversityManagement.Application.Common.Models;
using DataAnnotationsValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace UniversityManagement.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private const string DefaultUniqueViolationMessage = "A record with the same unique value already exists.";
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IOptions<JsonOptions> jsonOptions)
    {
        _next = next;
        _logger = logger;
        _jsonSerializerOptions = jsonOptions.Value.JsonSerializerOptions;
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
        List<string>? validationErrors = null;

        if (exception is ValidationException fluentValidationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = "Validation failed.";
            validationErrors = BuildValidationErrors(fluentValidationException);
        }
        else if (exception is DataAnnotationsValidationException dataAnnotationsValidationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = dataAnnotationsValidationException.Message;
        }
        else if (exception is UnauthorizedAccessException unauthorizedException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            message = string.IsNullOrWhiteSpace(unauthorizedException.Message)
                ? "Unauthorized"
                : unauthorizedException.Message;
        }
        else if (exception is InvalidOperationException invalidOperationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = string.IsNullOrWhiteSpace(invalidOperationException.Message)
                ? "The request could not be processed."
                : invalidOperationException.Message;
        }
        else if (exception is KeyNotFoundException notFoundException)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            message = notFoundException.Message;
        }
        else if (exception is DbUpdateException dbUpdateException &&
                 TryHandleDbUpdateException(dbUpdateException, out var uniqueConstraintMessage))
        {
            statusCode = StatusCodes.Status409Conflict;
            message = uniqueConstraintMessage;
        }

        _logger.LogError(
            exception,
            "Unhandled exception processing {Method} {Path}. TraceId: {TraceId}. Responding with {StatusCode}",
            context.Request.Method,
            context.Request.Path,
            context.TraceIdentifier,
            statusCode);

        var response = ApiResponse<object>.Fail(message, validationErrors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = JsonSerializer.Serialize(response, _jsonSerializerOptions);
        await context.Response.WriteAsync(payload);
    }

    private static List<string> BuildValidationErrors(ValidationException exception)
    {
        var errors = new List<string>();

        foreach (var failure in exception.Errors)
        {
            if (!string.IsNullOrWhiteSpace(failure?.ErrorMessage))
            {
                errors.Add(failure.ErrorMessage);
            }
        }

        if (errors.Count == 0 && !string.IsNullOrWhiteSpace(exception.Message))
        {
            errors.Add(exception.Message);
        }

        return errors;
    }

    private static bool TryHandleDbUpdateException(DbUpdateException exception, out string friendlyMessage)
    {
        friendlyMessage = DefaultUniqueViolationMessage;

        if (exception.InnerException is not PostgresException postgresException)
        {
            return false;
        }

        if (!string.Equals(postgresException.SqlState, PostgresErrorCodes.UniqueViolation, StringComparison.Ordinal))
        {
            return false;
        }

        friendlyMessage = ResolveUniqueViolationMessage(postgresException.ConstraintName);
        return true;
    }

    private static string ResolveUniqueViolationMessage(string? constraintName)
    {
        if (string.IsNullOrWhiteSpace(constraintName))
        {
            return DefaultUniqueViolationMessage;
        }

        if (UniqueConstraintErrorMessages.TryGetValue(constraintName, out var message))
        {
            return message;
        }

        return DefaultUniqueViolationMessage;
    }


    private static readonly Dictionary<string, string> UniqueConstraintErrorMessages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["IX_Classes_Name"] = "A class with the same name already exists.",
        ["IX_Courses_Name"] = "A course with the same name already exists.",
        ["IX_Users_Email"] = "A user with the same email address already exists.",
        ["IX_CourseClasses_CourseId_ClassId"] = "This course is already linked to the specified class.",
        ["IX_UserCourses_UserId_CourseId"] = "The user is already enrolled in the specified course.",
        ["IX_UserCourseClass_UserId_CourseId_ClassId"] = "The user is already assigned to this course and class combination."
    };
}

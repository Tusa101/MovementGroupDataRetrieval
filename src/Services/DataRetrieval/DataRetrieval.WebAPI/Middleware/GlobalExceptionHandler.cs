﻿using System.Net;
using Application.Exceptions;
using Domain.CommonConstants;
using Domain.Exceptions;
using Infrastructure.Configuration.Extensions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
namespace DataRetrieval.WebAPI.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var message = "Undefined exception";
        var title = "An error occurred while processing your request.";
        var statusCode = 500;
        var stackTrace = exception.StackTrace;

        if (exception.GetType().IsSubclassOf(typeof(BaseException)) || exception is BaseException)
        {
            message = exception.Message;
            title = (exception as BaseException)!.Code;
        }

        var errors = Array.Empty<ApiError>();

        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors
                .SelectMany(kvp => kvp.Value,
                (kvp, value) => new ApiError(kvp.Key, value))
                .ToArray();
        }

        if (exception is BaseException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
        }

        if (exception is DuplicateValueException || exception.GetType().IsSubclassOf(typeof(DuplicateValueException)))
        {
            statusCode = (int)HttpStatusCode.Conflict;
        }
        if (exception is NotFoundException || exception.GetType().IsSubclassOf(typeof(NotFoundException)))
        {
            statusCode = (int)HttpStatusCode.NotFound;
        }

        var problem = new ExtendedProblemDetails
        {
            Title = title,
            Detail = message,
            Status = statusCode,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231",
            Errors = errors
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response
            .WriteAsJsonAsync(problem, cancellationToken);

        var errorMessage = string.Format(null, LoggingTemplates.ErrorTemplate, message, stackTrace);

#pragma warning disable CA2254 // Template should be a static expression
        _logger.LogError(errorMessage);
#pragma warning restore CA2254 // Template should be a static expression

        return true;
    }
}

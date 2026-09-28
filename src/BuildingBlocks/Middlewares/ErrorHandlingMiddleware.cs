using BuildingBlocks.Exceptions;
using FluentValidation;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BuildingBlocks.Middlewares;

public class ErrorHandlingMiddleware
        (ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleErrorAsync(context, 401, "Unauthorized", ex.Message);
        }
        catch (ConflictException ex)
        {
            await HandleErrorAsync(context, 409, "Conflict", ex.Message);
        }
        catch (InsufficientStockException ex)
        {
            await HandleErrorAsync(context, 409, "Conflict", ex.Message);
        }
        catch (ForbiddenException ex)
        {
            await HandleErrorAsync(context, 403, "Forbidden", ex.Message);
        }
        catch (NotFoundException ex)
        {
            await HandleErrorAsync(context, 404, "Not Found", ex.Message);
        }
        catch (BadHttpRequestException ex)
        {
            await HandleErrorAsync(context, 400, "Bad Request", ex.Message);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new ErrorDetails(e.ErrorMessage, e.PropertyName));
            await HandleErrorAsync(context, 400, "Validation Error", errors);
        }
        catch (RpcException ex)
        {
            var status = ex.StatusCode switch
            {
                StatusCode.NotFound => HttpStatusCode.NotFound,
                _ => HttpStatusCode.BadRequest,
            };

            await HandleErrorAsync(context, (int)status, "RPC Error", 
               ex.Status.Detail ?? ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            await HandleErrorAsync(context, 500, "Internal Server Error", ex.Message);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, int statusCode, string type, string message)
    {
        logger.LogWarning($"Error: {type} - {message}");
        var response = new ErrorResponse(type, [new ErrorDetails(message)]);
        await WriteResponseAsync(context, statusCode, response);
    }

    private async Task HandleErrorAsync(HttpContext context, int statusCode, string type, IEnumerable<ErrorDetails> errors)
    {
        var response = new ErrorResponse(type, errors);
        await WriteResponseAsync(context, statusCode, response);
    }

    private async Task WriteResponseAsync(HttpContext context, int statusCode, ErrorResponse response)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(response);
    }

    private class ErrorResponse(string type, IEnumerable<ErrorDetails> errors)
    {
        public string Type { get; set; } = type;
        public IEnumerable<ErrorDetails> Errors { get; set; } = errors;
    }
    private record ErrorDetails(string Message, string? Field = null);
}

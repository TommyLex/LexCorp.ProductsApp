using LexCorp.Results.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LexCorp.ProductsApp.Api.Middlewares
{
  /// <summary>
  /// Middleware for handling exceptions globally in the application.
  /// Logs the exception and returns a standardized error response.
  /// </summary>
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger for logging exceptions.</param>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware to handle the HTTP request and catch any unhandled exceptions.
    /// </summary>
    /// <param name="httpContext">The HTTP context of the current request.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An unexpected error occurred.");
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    /// <summary>
    /// Handles the exception by setting the response status code and returning a standardized error response.
    /// </summary>
    /// <param name="context">The HTTP context of the current request.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      var errorResponse = new ResultInfoDto(false, new[] { exception.Message });
      var errorJson = JsonConvert.SerializeObject(errorResponse);

      return context.Response.WriteAsync(errorJson);
    }
  }
}
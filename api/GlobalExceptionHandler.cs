using System;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace claude_tech_test;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    logger.LogError(exception, "An unhandled exception occurred");

    var problemDetails = new ProblemDetails
    {
      Status = (int)HttpStatusCode.InternalServerError,
      Title = "An unexpected error occurred",
    };

    httpContext.Response.StatusCode = problemDetails.Status.Value;
    httpContext.Response.ContentType = "application/json";

    await httpContext.Response
        .WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}

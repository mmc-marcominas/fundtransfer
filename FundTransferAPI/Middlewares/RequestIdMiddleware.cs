/// <summary>
/// Constants
/// </summary>
public static class Constants
{
  /// <summary>
  /// RequestId Header name
  /// </summary>
  public const string RequestIdHeaderName = "X-Request-ID";
}

/// <summary>
/// RequestId Middleware
/// </summary>
public class RequestIdMiddleware
{
  private readonly RequestDelegate _next;

  /// <summary>
  /// RequestId Middleware constructor
  /// </summary>
  /// <param name="next"><see cref="RequestDelegate"/></param>
  public RequestIdMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  /// <summary>
  /// Invoke Async implementation
  /// </summary>
  /// <param name="context"><see cref="HttpContext"/></param>
  /// <returns></returns>
  public async Task InvokeAsync(HttpContext context)
  {
    if (string.IsNullOrWhiteSpace(context.Request.Headers[Constants.RequestIdHeaderName]))
    {
      var requestId = Guid.NewGuid().ToString();
      context.Request.Headers[Constants.RequestIdHeaderName] = requestId;
    }

    await _next(context);
  }
}

/// <summary>
/// RequestId Middleware Extensions
/// </summary>
public static class RequestIdMiddlewareExtensions
{
  /// <summary>
  /// Use RequestId Middleware <see cref="IApplicationBuilder"/> extension
  /// </summary>
  /// <param name="builder"><see cref="IApplicationBuilder"/></param>
  /// <returns></returns>
  public static IApplicationBuilder UseRequestIdMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<RequestIdMiddleware>();
  }
  
  /// <summary>
  /// Get RequestId <see cref="HttpContext"/> extension
  /// </summary>
  /// <param name="context"><see cref="HttpContext"/></param>
  /// <returns></returns>
  public static string GetRequestId(this HttpContext context)
  {
    return $"{context.Request.Headers[Constants.RequestIdHeaderName]}";
  }
}

public static class Constants
{
  public const string RequestIdHeaderName = "X-Request-ID";
}

public class RequestIdMiddleware
{
  private readonly RequestDelegate _next;

  public RequestIdMiddleware(RequestDelegate next)
  {
    _next = next;
  }

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

public static class RequestIdMiddlewareExtensions
{
  public static IApplicationBuilder UseRequestIdMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<RequestIdMiddleware>();
  }
  
  public static string GetRequestId(this HttpContext context)
  {
    return $"{context.Request.Headers[Constants.RequestIdHeaderName]}";
  }
}

using System.Security.Claims;
using VirtualMenuAPI.SSEMiddleware.CustomerSSE;

namespace VirtualMenuAPI.SSEMiddleware.BaristaSSE
{

  public static class SseHolderMapper
  {
    public static IApplicationBuilder MapBaristaSseHolder(this IApplicationBuilder app, PathString path)
    {
      return app.Map(path, (app) => app.UseMiddleware<SseBaristaMiddleware>());
    }
  }
  public class SseBaristaMiddleware
  {
    private readonly RequestDelegate next;
    private readonly IBaristaSseHolder sse;
    public SseBaristaMiddleware(RequestDelegate next,
        IBaristaSseHolder sse)
    {
      this.next = next;
      this.sse = sse;
    }
    public async Task InvokeAsync(HttpContext context)
    {
      if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
      {
        if (context.User.IsInRole("Barista"))
        {
          await sse.AddAsync(context);
        }
        else
        {
          context.Response.StatusCode = 403;
          await context.Response.WriteAsync("Access Denied: You do not have the required role.");
        }
      }
      else
      {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized: You must be authenticated to access this resource.");
      }
    }

  }
}

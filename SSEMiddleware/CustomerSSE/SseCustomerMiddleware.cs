﻿using VirtualMenuAPI.SSEMiddleware.CustomerSSE;

namespace VirtualMenuAPI.SSE.MiddleWare
{

  public static class CustomerSseHolderMapper
  {
    public static IApplicationBuilder MapCustomerSseHolder(this IApplicationBuilder app, PathString path)
    {
      return app.Map(path, (app) => app.UseMiddleware<SseCustomerMiddleware>());
    }
  }
  public class SseCustomerMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ICustomerSseHolder sse;
    public SseCustomerMiddleware(RequestDelegate next,
        ICustomerSseHolder sse)
    {
      this.next = next;
      this.sse = sse;
    }
    public async Task InvokeAsync(HttpContext context)
    {
      if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
      {
        if (context.User.IsInRole("Customer"))
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

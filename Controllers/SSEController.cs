using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Services;
using System;

namespace VirtualMenuAPI.Controllers
{

  [ApiController]
  [Route("[controller]")]
  public class SSEController : ControllerBase
  {
    private readonly IHostApplicationLifetime _hostLifetime;
    private readonly IOrderQueueService _orderQueueService;
    public SSEController(IHostApplicationLifetime hostLifetime, IOrderQueueService orderQueueService)
    {
      _hostLifetime = hostLifetime;
      _orderQueueService = orderQueueService;
    }
    [HttpGet]
    //[Authorize(Roles = UserRoles.Barista)]
    public async Task<IActionResult> BaristaSSE()
    {
      Response.Headers.Add("Content-Type", "text/event-stream");
      Response.Headers.Add("Cache-Control", "no-cache");
      Response.Headers.Add("Connection", "keep-alive");

      var cancelToken = _hostLifetime.ApplicationStopping;
      await Response.WriteAsync("data: Hello there\r\r");

      while (!cancelToken.IsCancellationRequested)
      {
        if (_orderQueueService.TryDequeueMessage(out var eventMessage))
        {
          string eventBytes = JsonSerializer.Serialize(eventMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
          await Response.WriteAsync($"data: {eventBytes}\r\r");
          await Response.Body.FlushAsync();
          }
        await Task.Delay(5000);
      }

      return new EmptyResult();
    }

  }
}

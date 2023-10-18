//using Azure;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using System.Collections.Concurrent;
//using System.Text;
//using System.Text.Json;
//using VirtualMenuAPI.Dto;
//using VirtualMenuAPI.Services;
//using System;
//using VirtualMenuAPI.SSEMiddleware.CustomerSSE;

//namespace VirtualMenuAPI.Controllers
//{

//  [ApiController]
//  [Route("[controller]")]
//  public class SSEController : ControllerBase
//  {
//    private readonly IHostApplicationLifetime _hostLifetime;
//    private readonly IOrderQueueService _orderQueueService;
//    private readonly ISseHolder _sseHolder;
//    private readonly ILogger<SSEController> logger;
//    public SSEController(IHostApplicationLifetime hostLifetime, IOrderQueueService orderQueueService, ISseHolder sseHolder, ILogger<SSEController> logger)
//    {
//      _hostLifetime = hostLifetime;
//      _orderQueueService = orderQueueService;
//      _sseHolder = sseHolder;
//      this.logger = logger;
//    }
//    [HttpGet]
//    //[Authorize(Roles = UserRoles.Barista)]
//    public async Task<IActionResult> BaristaSSE()
//    {
//      Response.Headers.Add("Content-Type", "text/event-stream");
//      Response.Headers.Add("Cache-Control", "no-cache");
//      Response.Headers.Add("Connection", "keep-alive");

//      var cancelToken = _hostLifetime.ApplicationStopping;
//      await Response.WriteAsync("data: Hello there\r\r");
//      var index = 0;
//      while (!cancelToken.IsCancellationRequested)
//      {
//        if (_orderQueueService.TryDequeueMessage(out var eventMessage))
//        {
          
//          string eventBytes = JsonSerializer.Serialize(eventMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
//          await Response.WriteAsync($"data: {eventBytes}\r\r");
//          await Response.Body.FlushAsync();
//        }

//        await Response.WriteAsync($"data: {index++} : {DateTime.Now:mm:ss}\r\r");
//        await Task.Delay(2000);
//      }

//      return new EmptyResult();
//    }

//    //[HttpPost]
//    //[Route("/sse/message")]
//    //public async Task<string> SendMessage([FromBody] SseMessage? message)
//    //{
//    //  if (string.IsNullOrEmpty(message?.Id) ||
//    //      string.IsNullOrEmpty(message?.Message))
//    //  {
//    //    return "No messages";
//    //  }
//    //  await _sseHolder.SendMessageAsync(message);
//    //  return "";
//    //}

//  }
//}

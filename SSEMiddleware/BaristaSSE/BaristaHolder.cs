using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Events;

namespace VirtualMenuAPI.SSEMiddleware.BaristaSSE
{
  public record SseClient(HttpResponse Response, CancellationTokenSource Cancel);
  public class BaristaHolder : IBaristaSseHolder
  {
    private readonly ILogger<BaristaHolder> logger;
    private readonly ConcurrentDictionary<string, SseClient> clients = new();


    public BaristaHolder(ILogger<BaristaHolder> logger,
        IHostApplicationLifetime applicationLifetime)
    {
      this.logger = logger;
      applicationLifetime.ApplicationStopping.Register(OnShutdown);
    }
    public async Task AddAsync(HttpContext context)
    {
      var clientId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (clientId is null)
      {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
        return;
      }
      var cancel = new CancellationTokenSource();
      var client = new SseClient(Response: context.Response, Cancel: cancel);
      logger.LogError($"New Client added, {clientId}");
      if (clients.TryAdd(clientId, client))
      {
        EchoAsync(client);
        context.RequestAborted.WaitHandle.WaitOne();
        RemoveClient(clientId);
        await Task.FromResult(true);
      }
    }
    public async Task SendMessageAsync<T>(T @event) where T : SseEvent
    {

      foreach (var c in clients)
      {
        //var messageJson = JsonSerializer.Serialize(@event.GetContent());
        await c.Value.Response.WriteAsync($"data: {@event.GetContent()}\r\r", c.Value.Cancel.Token);
        await c.Value.Response.Body.FlushAsync(c.Value.Cancel.Token);
      }
    }


    private async void EchoAsync(SseClient client)
    {
      try
      {
        var clientIdJson = JsonSerializer.Serialize("hi Barista");
        client.Response.Headers.Add("Content-Type", "text/event-stream");
        client.Response.Headers.Add("Cache-Control", "no-cache");
        client.Response.Headers.Add("Connection", "keep-alive");
        // Send ID to client-side after connecting
        await client.Response.WriteAsync($"data: {clientIdJson}\r\r", client.Cancel.Token);
        await client.Response.Body.FlushAsync(client.Cancel.Token);
      }
      catch (OperationCanceledException ex)
      {
        logger.LogError($"Exception {ex.Message}");
      }

    }
    private void OnShutdown()
    {
      var tmpClients = new List<KeyValuePair<string, SseClient>>();
      foreach (var c in clients)
      {
        c.Value.Cancel.Cancel();
        tmpClients.Add(c);
      }
      foreach (var c in tmpClients)
      {
        clients.TryRemove(c);
      }
    }
    public void RemoveClient(string id)
    {
      var target = clients.FirstOrDefault(c => c.Key == id);
      if (string.IsNullOrEmpty(target.Key))
      {
        return;
      }
      target.Value.Cancel.Cancel();
      clients.TryRemove(target);
    }

  }
}

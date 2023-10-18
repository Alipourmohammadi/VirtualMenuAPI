using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Events;

namespace VirtualMenuAPI.SSEMiddleware.CustomerSSE
{
  public record SseClient(HttpResponse Response, CancellationTokenSource Cancel);
  public class CustomerHolder : ISseHolder
  {
    private readonly ILogger<CustomerHolder> logger;
    private readonly ConcurrentDictionary<string, SseClient> clients = new();


    public CustomerHolder(ILogger<CustomerHolder> logger,
        IHostApplicationLifetime applicationLifetime)
    {
      this.logger = logger;
      applicationLifetime.ApplicationStopping.Register(OnShutdown);
    }
    public async Task AddAsync(HttpContext context)
    {
      var clientId = context.User.FindFirstValue("Identity");
      if (clientId is not null)
      {
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
    }
    public async Task SendMessageAsync<T>(T @event) where T : SseEvent
    {
      //logger.LogWarning($"Send Message {orderStatus.Message}");
      if (clients.TryGetValue(@event.Id.ToString(), out var c))
      {
        var messageString = "Empty Message";
        var messageProperty = typeof(T).GetProperty("Status");
        if (messageProperty != null)
        {
          var messageValue = messageProperty.GetValue(@event);
          messageString = JsonSerializer.Serialize(messageValue);
        }
        else
        {
          messageString = JsonSerializer.Serialize(@event);
        }
        await c.Response.WriteAsync($"data: {messageString}\r\r", c.Cancel.Token);
        await c.Response.Body.FlushAsync(c.Cancel.Token);
      }
      else
      {
        // alert the barista that the user did not get the action
        throw new Exception("user Doesn't Exist");
      }

    }


    private async void EchoAsync(SseClient client)
    {
      try
      {
        var clientIdJson = JsonSerializer.Serialize("hello there");
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

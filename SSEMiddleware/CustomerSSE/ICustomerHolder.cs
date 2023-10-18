using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Events;

namespace VirtualMenuAPI.SSEMiddleware.CustomerSSE
{
  public interface ISseHolder
  {
    Task AddAsync(HttpContext context);
    Task SendMessageAsync<T>(T message) where T: SseEvent;
  }
}

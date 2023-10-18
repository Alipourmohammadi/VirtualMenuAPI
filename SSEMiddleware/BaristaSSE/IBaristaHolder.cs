using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Events;

namespace VirtualMenuAPI.SSEMiddleware.BaristaSSE
{
  public interface IBaristaSseHolder
  {
    Task AddAsync(HttpContext context);
    Task SendMessageAsync<T>(T message) where T : SseEvent;
  }
}

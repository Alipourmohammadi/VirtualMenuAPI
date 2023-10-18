using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Events;

namespace VirtualMenuAPI.SSEMiddleware.CustomerSSE
{
  public interface ICustomerSseHolder
  {
    Task AddAsync(HttpContext context);
    Task SendMessageAsync<T>(T message) where T: SseEvent;
  }
}

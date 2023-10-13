using VirtualMenuAPI.Data.Dtos;

namespace VirtualMenuAPI.SSEMiddleware.CustomerSSE
{
  public interface ISseHolder
  {
    Task AddAsync(HttpContext context);
    Task SendMessageAsync(SseOrderStatusDto message);
  }
}

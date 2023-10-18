using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Data.Events
{
  public class OrderStatusUpdatedEvent: SseEvent
  {
    public OrderState Status { get; set; }

    public override string GetContent()
    {
      return $"{Status}";
    }
  }
}

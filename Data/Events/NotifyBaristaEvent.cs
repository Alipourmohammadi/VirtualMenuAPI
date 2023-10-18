using Microsoft.Extensions.Logging;
using System.Text.Json;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Data.Events
{
  public class NotifyBaristaEvent : SseEvent
  {

    public Order OrderDto { get; set; }

    public override string GetContent()
    {
      var output = JsonSerializer.Serialize(OrderDto);
      return $"{output}";
    }

  }
}

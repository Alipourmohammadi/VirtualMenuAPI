using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Data.Dtos
{
  public class SseOrderStatusDto
  {
    [JsonPropertyName("id")]
    public Guid Identity { get; init; } 
    [JsonPropertyName("message")]
    public string Message { get; init; } = null!;
  }
}

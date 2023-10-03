using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Models
{
  public class OrderItem
  {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Count { get; set; }
  }
}
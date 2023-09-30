using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Models
{
  public class Product
  {
    public int Id { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int Price { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore]
    public Category Category { get; set; } = new();
  }
}
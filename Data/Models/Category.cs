using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Models
{
  public class Category
  {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    [JsonIgnore]
    public List<Product> Products { get; set; } = new();
  }
}
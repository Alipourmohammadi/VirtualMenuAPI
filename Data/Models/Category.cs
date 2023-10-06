using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Models
{
  public class Category
  {
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Value length is Out of Range")]
    public string Title { get; set; } = string.Empty;
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Value length is Out of Range")]
    public string Image { get; set; } = string.Empty;
    [JsonIgnore]
    public List<Product> Products { get; set; } = new();
  }
}
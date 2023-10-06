using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Models
{
  public class Product
  {
    private const string _errorMessage = "Out Of Range";
    public int Id { get; set; }

    [StringLength(100, MinimumLength = 5, ErrorMessage = _errorMessage)]
    public string Image { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 5, ErrorMessage = _errorMessage)]
    public string Title { get; set; } = string.Empty;

    [Range(0, 10_000, ErrorMessage = _errorMessage)]
    public int Duration { get; set; }

    [Range(0, 99_999_999, ErrorMessage = _errorMessage)]
    public int Price { get; set; }
    public int CategoryId { get; set; }
    [JsonIgnore]
    public Category Category { get; set; } = new();
  }
}
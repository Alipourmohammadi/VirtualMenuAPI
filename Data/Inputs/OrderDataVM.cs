using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.ViewModels{
  public class OrderDataIN
  {
    [Range(0, 200, ErrorMessage = "Invalid Data")]
    public int TableNumber { get; set; }

    [Range(0, 1, ErrorMessage = "Invalid Data")]
    public ServeType ServeType { get; set; } = ServeType.Present;

    [Range(0, 2, ErrorMessage = "Invalid Data")]
    public OrderState OrderState { get; set; }
    [Required]
    public List<OrderItem> Items { get; set; } = new();
  }
}
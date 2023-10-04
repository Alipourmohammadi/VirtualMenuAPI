using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.ViewModels{
  public class OrderDataVM
  {
    public int TableNumber { get; set; }
    public ServeType ServeType { get; set; } = ServeType.Present;
    public OrderState OrderState { get; set; } 
    [Required]
    public List<OrderItem> Items { get; set; } = new();
  }
}
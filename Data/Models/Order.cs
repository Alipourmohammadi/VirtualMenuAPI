using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Models
{
  public enum ServeType
  {
    Present,
    TakAway
  }
  public enum OrderState
  {
    OrderJustSet,
    OrderInProgress,
    OrderDone
  }
  public class Order
  {
    public int Id { get; set; }
    public DateTime SubmitDate { get; set; } = DateTime.Now;
    public int TableNumber { get; set; }
    public ServeType ServeType { get; set; } = ServeType.Present;
    public OrderState OrderState { get; set; } 
    [Required]
    public List<OrderItem> Items { get; set; } = new();
    [JsonIgnore]
    [Required]
    public Customer Customer { get; set; } = new();
  }
}
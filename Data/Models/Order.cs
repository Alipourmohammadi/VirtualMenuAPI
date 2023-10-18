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
    OrderJustSet, //Initialized
    OrderInProgress, //InProgress
    OrderDone //Done
  }
  public class Order
  {
    public int Id { get; set; }

    public DateTime SubmitDate { get; set; } = DateTime.Now;

    [Range(0, 50, ErrorMessage = "Out of Range!")]
    public int TableNumber { get; set; }

    [Range(0, 1, ErrorMessage = "Out of Range!")]
    public ServeType ServeType { get; set; } = ServeType.Present;

    [Range(0, 2, ErrorMessage = "Out of Range!")]
    public OrderState OrderState { get; set; } // State
    [Required]
    public List<OrderItem> Items { get; set; } = new();
    [JsonIgnore]
    public Customer Customer { get; set; } = new();

    //public void InitializeOrder()
    //{
    //  OrderState = OrderState.OrderJustSet;
    //}


    //public void SetInProgress()
    //{

    //}


  }
}
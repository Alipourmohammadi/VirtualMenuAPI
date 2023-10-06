using System.ComponentModel.DataAnnotations;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Data.Inputs
{
  public class OrderStateIN
  {
    public int OrderId { get; set; }

    [Range(0, 2, ErrorMessage = "Invalid Data")]
    public OrderState OrderState { get; set; }
  }
}

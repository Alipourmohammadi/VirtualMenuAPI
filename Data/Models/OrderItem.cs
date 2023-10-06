using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Models
{
  public class OrderItem
  {
    public int Id { get; set; }
    [StringLength(50,MinimumLength =5,ErrorMessage ="Value length is Out of Range")]
    public string Title { get; set; } = string.Empty;
    [Range(0,50,ErrorMessage ="Out of Range")]
    public int Count { get; set; }
    [Range(0,99_999_999,ErrorMessage ="Out of Range")]
    public int Price { get; set; }
  }
}
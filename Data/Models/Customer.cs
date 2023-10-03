using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Models
{
  public class Customer
  {
    [Key]
    public int Id { get; set; }
    public Guid Identity { get; set; }
    public DateTime DateCreated { get; set; }= DateTime.Now;
    [Required]
    public List<Order> Orders { get; set; } = new();
  }
}
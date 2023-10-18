using System.ComponentModel.DataAnnotations;
using VirtualMenuAPI.Data.Models.Users;

namespace VirtualMenuAPI.Models
{
  public class Customer :Account
  {
    public Guid Identity { get; set; }

    //TODO: remove required
    [Required]
    public List<Order> Orders { get; set; } = new();
  }
}
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Identity;

namespace VirtualMenuAPI.Models
{
  public class Customer :IdentityUser
  {
    public Guid Identity { get; set; }
    public DateTime DateCreated { get; set; }= DateTime.Now;
    [Required]
    public List<Order> Orders { get; set; } = new();
  }
}
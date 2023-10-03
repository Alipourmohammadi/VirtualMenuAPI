using Microsoft.AspNetCore.Identity;

namespace VirtualMenuAPI.Models;
public class Manager : IdentityUser
{
  public string ComponyName { get; set; } = string.Empty;
  public DateTime DateCreated { get; set; } = DateTime.Now;
  public Customer Customers { get; set; } = new();
}
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.ViewModels
{
  public class UserDataVM
  {
    public string? Token { get; set; }
    public OrderDataVM Order { get; set; } = new();
  }
}
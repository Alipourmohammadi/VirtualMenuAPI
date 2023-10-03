using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.ViewModels
{
  public class SetOrderVM
  {
    public string? Token { get; set; }
    public Order Order { get; set; } = new();
  }
}

using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.ViewModels
{
  public record struct MenuDataDto(List<Category> CategoryItems,List<Product> ProductItems);
}
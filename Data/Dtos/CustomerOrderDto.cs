using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Dto
{
  public record struct CustomerOrderDto(
    int Id,
    int TableNumber,
    ServeType ServeType,
    OrderState OrderState,
    List<OrderItem> Items
    );
}
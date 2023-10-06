using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Services.BaristaServices
{
  public interface IBaristaService
  {
    Task<List<CustomerOrderDto>> GetOrders();
    Task<CustomerOrderDto> ChangeOrderState(OrderStateIN orderState);
  }
}

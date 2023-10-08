using VirtualMenuAPI.Dto;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Services.CustomerService
{
  public interface ICustomerService
  {
    Task<CustomerOrderDto> SetOrder(OrderDataIN order,string userId);
    Task<List<CustomerOrderDto>> GetCustomerOrderData(string userId);
  }
}
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Services.CustomerService
{
  public interface ICustomerService
  {
    Task<bool> SetOrder(OrderDataIN order,string userId);
    Task<List<CustomerOrderDto>> GetCustomerOrderData(string userId);
  }
}
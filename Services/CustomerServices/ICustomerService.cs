using VirtualMenuAPI.Dto;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Services.CustomerService
{
  public interface ICustomerService
  {
    Task<bool> SetOrder(UserDataVM orderVM);
    Task<List<CustomerOrderDto>> GetCustomerOrderData(TokenVM tokenVM);
  }
}
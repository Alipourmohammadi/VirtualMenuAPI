using System.IdentityModel.Tokens.Jwt;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Services.CustomerService
{
  public class CustomerService : ICustomerService
  {
    private readonly UserManager<Customer> _customerManager;
    public CustomerService(UserManager<Customer> customerManager, IConfiguration configuration)
    {
      _customerManager = customerManager;
    }

    public async Task<bool> SetOrder(UserDataVM userData)
    {
      var TokenHandler = new JwtSecurityTokenHandler();
      var jwt = TokenHandler.ReadJwtToken(userData.Token);
      string identity = jwt.Claims.First(c => c.Type == "Identity").Value;
      var customer = await _customerManager.Users.FirstOrDefaultAsync(x => x.Identity.ToString() == identity);
      if (customer == null)
        return false;
      var newOrder = new Order()
      {
        TableNumber = userData.Order.TableNumber,
        Items = userData.Order.Items,
        ServeType = userData.Order.ServeType
      };
      customer.Orders.Add(newOrder);
      return true;
    }
    public async Task<List<CustomerOrderDto>> GetCustomerOrderData(TokenVM tokenVM)
    {
      var TokenHandler = new JwtSecurityTokenHandler();
      var jwt = TokenHandler.ReadJwtToken(tokenVM.Token);
      string identity = jwt.Claims.First(c => c.Type == "Identity").Value;
      // Console.WriteLine(identity);
      var customer = await _customerManager.Users.FirstOrDefaultAsync(x => x.Identity.ToString() == identity);
      if (customer is null)
        throw new Exception("couldn't find the user");
      
      return customer.Orders.Adapt<List<CustomerOrderDto>>();
    }
    
  }
}

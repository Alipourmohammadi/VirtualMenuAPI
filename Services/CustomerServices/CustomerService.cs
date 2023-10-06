using System.IdentityModel.Tokens.Jwt;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Data.Models.Users;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Services.CustomerService
{
  public class CustomerService : ICustomerService
  {
    private readonly DataContext _context;
    public CustomerService(DataContext context)
    {
      _context = context;
    }

    public async Task<bool> SetOrder(OrderDataIN order, string userId)
    {
      var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Identity.ToString() == userId);
      if (customer == null)
        return false;
      var newOrder = new Order()
      {
        TableNumber = order.TableNumber,
        ServeType = order.ServeType,
        Items = order.Items,
      };
      customer.Orders.Add(newOrder);
      _context.Customers.Update(customer);
      await _context.SaveChangesAsync();
      return true;
    }
    public async Task<List<CustomerOrderDto>> GetCustomerOrderData(string userId)
    {
      var customer = await _context.Customers.Include(x=>x.Orders).ThenInclude(z=>z.Items).FirstOrDefaultAsync(x => x.Identity.ToString() == userId);
      if (customer is null)
        throw new Exception("couldn't find the user");
      var customerOrders = customer.Orders;
      return customerOrders.Adapt<List<CustomerOrderDto>>();
    }

  }
}

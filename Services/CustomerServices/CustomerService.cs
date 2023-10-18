using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Data.Events;
using VirtualMenuAPI.Data.Models.Users;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.SSEMiddleware.BaristaSSE;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Services.CustomerService
{
  public class CustomerService : ICustomerService
  {
    private readonly DataContext _context;
    private readonly IBaristaSseHolder _baristaSseHolder;

    public CustomerService(DataContext context, IBaristaSseHolder baristaSseHolder)
    {
      _context = context;
      _baristaSseHolder = baristaSseHolder;
    }

    public async Task<CustomerOrderDto> SetOrder(OrderDataIN order, string userId)
    {
      var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Identity.ToString() == userId);
      if (customer == null)
        throw new Exception("Customer Not Found");
      var newOrder = new Order()
      {
        TableNumber = order.TableNumber,
        ServeType = order.ServeType,
        Items = order.Items,
      };
      customer.Orders.Add(newOrder);
      var notification = new NotifyBaristaEvent()
      {
        OrderDto = newOrder
      };
      await _baristaSseHolder.SendMessageAsync(notification);
      _context.Customers.Update(customer);
      await _context.SaveChangesAsync();
      //var orders = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);
      return customer.Orders.FirstOrDefault(x => x.Id == newOrder.Id).Adapt<CustomerOrderDto>();
    }
    public async Task<List<CustomerOrderDto>> GetCustomerOrderData(string userId)
    {
      var customer = await _context.Customers.Include(x => x.Orders).ThenInclude(z => z.Items).FirstOrDefaultAsync(x => x.Identity.ToString() == userId);
      if (customer is null)
        throw new Exception("couldn't find the user");
      var customerOrders = customer.Orders;
      return customerOrders.Adapt<List<CustomerOrderDto>>();
    }

  }
}

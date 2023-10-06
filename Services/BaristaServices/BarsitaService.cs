using Mapster;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Dto;

namespace VirtualMenuAPI.Services.BaristaServices
{
  public class BarsitaService : IBaristaService
  {
    private readonly DataContext _Context;

    public BarsitaService(DataContext dataContext)
    {
      _Context = dataContext;
    }

    public async Task<List<CustomerOrderDto>> GetOrders()
    {
      var orders = await _Context.Orders.Include(x => x.Items).ToListAsync();
      if (orders is null)
        throw new Exception("No orders");
      return orders.Adapt<List<CustomerOrderDto>>();
    }
    public async Task<CustomerOrderDto> ChangeOrderState(OrderStateIN orderStateIn)
    {
      var order = await _Context.Orders.FirstOrDefaultAsync(x => x.Id == orderStateIn.OrderId);
      if (order is null)
        throw new Exception("Order doesn't Exist");
      order.OrderState = orderStateIn.OrderState;
      _Context.Orders.Update(order);
      await _Context.SaveChangesAsync();
      return order.Adapt<CustomerOrderDto>();
    }
  }
}

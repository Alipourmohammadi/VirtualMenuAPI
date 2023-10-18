using Mapster;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Events;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Dto;
using VirtualMenuAPI.SSEMiddleware.CustomerSSE;

namespace VirtualMenuAPI.Services.BaristaServices
{
  public class BarsitaService : IBaristaService
  {
    private readonly DataContext _Context;
    private readonly ISseHolder _sseHolder;

    public BarsitaService(DataContext dataContext, ISseHolder sseHolder)
    {
      _Context = dataContext;
      _sseHolder = sseHolder;
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
      var order = await _Context.Orders.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == orderStateIn.OrderId);
      if (order is null)
        throw new Exception("Order doesn't Exist");
      order.OrderState = orderStateIn.OrderState;
      // BestPractice: Raise event (OrderStatusUpdated)
      //TODO: CONFIG camel case JsonSerializer in startup
      var orderUpdatedEvent = new OrderStatusUpdatedEvent()
      {
        Id = order.Customer.Identity,
        Status = orderStateIn.OrderState
      };
      try
      {
        await _sseHolder.SendMessageAsync(orderUpdatedEvent);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
      _Context.Orders.Update(order);
      await _Context.SaveChangesAsync();
      return order.Adapt<CustomerOrderDto>();
    }
  }
}

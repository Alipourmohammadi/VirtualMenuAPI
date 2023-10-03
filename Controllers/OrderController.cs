using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VirtualMenuAPI.Data;

// using VirtualMenuAPI.Handler;
using VirtualMenuAPI.Models;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
  private readonly DataContext _dataContext;
  private readonly WebSocketHandler _webSocketHandler;

  public OrderController(DataContext dataContext, WebSocketHandler webSocketHandler)
  {
    _dataContext = dataContext;
    _webSocketHandler = webSocketHandler;
  }

  [HttpGet]
  public async Task<List<Order>> GetOrders()
  {
    return await _dataContext.Orders.Include(x=>x.Items).ToListAsync();
  }
  [HttpPost]
  public async Task<ActionResult<Order>> PostOrder(Order order)
  {
    if (!ModelState.IsValid)
      return NotFound("Invalid Data");
    await _dataContext.Orders.AddAsync(order);
    _dataContext.SaveChanges();
    var savedOrders = await _dataContext.Orders.ToListAsync();
    await _webSocketHandler.SendDataAsync(JsonSerializer.Serialize(savedOrders, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

    return Ok("New Order Added");
  }

}

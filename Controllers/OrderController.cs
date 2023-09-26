using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
// using VirtualMenuAPI.Handler;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.Repository;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
  private readonly IOrderRepository _orderRepo;
  private readonly WebSocketHandler _webSocketHandler; // Inject WebSocketHandler

  public OrderController(IOrderRepository orderRepo, WebSocketHandler webSocketHandler)
  {
    _orderRepo = orderRepo;
    _webSocketHandler = webSocketHandler;
  }

  [HttpGet]
  public List<Order> GetOrders()
  {
    return _orderRepo.GetAllOrders();
  }
  [HttpPost]
  public async void PostOrder(Order order)
  {
    // Save the order to the repository
    var savedOrders = _orderRepo.SetOrder(order);

    // Notify WebSocket clients about the new order
    // var orderNotification = $"New order created: {order.Id}";

    await _webSocketHandler.SendDataAsync(JsonSerializer.Serialize(savedOrders));

    // return savedOrders;
  }

}

using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.Repository;

namespace VirtualMenuAPI.Controller;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
  private readonly IOrderRepository _orderRepo;
  public OrderController(IOrderRepository orderRepo)
  {
    _orderRepo = orderRepo;
  }
  [HttpPost]
  public List<Order> PostOrder(Order order)
  {
    return _orderRepo.SetOrder(order);
  }
  [HttpGet]
  public List<Order> GetOrders()
  {
    return _orderRepo.GetAllOrders();
  }
}
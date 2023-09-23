
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Repository;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controller
{
  [ApiController]
  [Route("[controller]")]
  public class MenuDataController : ControllerBase
  {
    private MenuData Data;
    private readonly IOrderRepository _orderRepo;
    public MenuDataController(IOrderRepository orderRepo)
    {
      Data = new MenuData();
      _orderRepo = orderRepo;
    }
    [HttpGet]
    public MenuData GetMenuData()
    {
      return Data.GetMenuData(_orderRepo);
    }
  }
}
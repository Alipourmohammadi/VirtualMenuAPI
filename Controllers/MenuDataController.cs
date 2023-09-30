
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controller
{
  [ApiController]
  [Route("[controller]")]
  public class MenuDataController : ControllerBase
  {
    private readonly DataContext _dataContext;

    public MenuDataController(DataContext dataContext)
    {
      _dataContext = dataContext;
    }
    [HttpGet]
    public async Task<MenuDataDto> GetMenuData()
    {
      return new MenuDataDto
      (
        await _dataContext.Categories.ToListAsync(),
        await _dataContext.Products.ToListAsync()
      );
    }
  }
}
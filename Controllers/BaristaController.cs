using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Services.BaristaServices;

namespace VirtualMenuAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Authorize(Roles = UserRoles.Barista)]
  public class BaristaController : ControllerBase
  {
    private readonly IBaristaService _baristaService;

    public BaristaController(IBaristaService baristaService)
    {
      _baristaService = baristaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
      try
      {
        var result = await _baristaService.GetOrders();
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex);
      }
    }
    [HttpPost]
    public async Task<IActionResult> ChangeOrderState([FromBody] OrderStateIN orderStateIn)
    {
      if (!ModelState.IsValid) return BadRequest("Invalid model");
      try
      {
        var result = await _baristaService.ChangeOrderState(orderStateIn);
        return Ok(result);
      }
      catch (Exception ex)
      {

        return BadRequest(ex.Message);
      }
    }
  }
}

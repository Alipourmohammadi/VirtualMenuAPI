using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Services.CustomerService;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Authorize(Roles = UserRoles.Customer)]

  public class CustomerDataController : ControllerBase
  {
    private readonly ICustomerService _customerService;

    public CustomerDataController(ICustomerService customerService)
    {
      _customerService = customerService;
    }

    [HttpPost("order")]
    public async Task<IActionResult> PostOrder([FromBody] OrderDataIN order)
    {
      if (!ModelState.IsValid)
        return BadRequest("Please Provide Valid Field");
      var userId = User.FindFirst("Identity")?.Value;
      if (userId is null)
        return NotFound();
      var result = await _customerService.SetOrder(order, userId);
      if (result)
        return Ok("Order Set");
      else
        return BadRequest("couldn't set the Order");
    }
    [HttpGet]
    public async Task<IActionResult> GetCustomerOrderData()
    {

      if (!ModelState.IsValid)
        return BadRequest("Please provide a Valid Field");
      var userId = User.FindFirst("Identity")?.Value;
      Console.WriteLine(userId);
      if (userId is null)
        return NotFound();
      try
      {
        var Data = await _customerService.GetCustomerOrderData(userId);
        return Ok(Data);
      }
      catch (Exception e)
      {
        return BadRequest(e.ToString());
      }
    }

  }
}
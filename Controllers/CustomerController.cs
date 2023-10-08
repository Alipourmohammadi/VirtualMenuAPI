using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Services;
using VirtualMenuAPI.Services.CustomerService;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Authorize(Roles = UserRoles.Customer)]

  public class CustomerController : ControllerBase
  {
    private readonly ICustomerService _customerService;
    private readonly IOrderQueueService _orderQueueService;

    public CustomerController(ICustomerService customerService, IOrderQueueService orderQueueService)
    {
      _customerService = customerService;
      _orderQueueService = orderQueueService;
    }

    [HttpPost("order")]
    public async Task<IActionResult> PostOrder([FromBody] OrderDataIN order)
    {
      if (!ModelState.IsValid)
        return BadRequest("Please Provide Valid Field");
      var userId = User.FindFirst("Identity")?.Value;
      if (userId is null)
        return NotFound("user not found");
      try
      {
        var result = await _customerService.SetOrder(order, userId);
        _orderQueueService.EnqueueMessage(result);
        return Ok("Order Set");
      }
      catch (Exception ex)
      {
        return NotFound(ex.Message);
      }
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
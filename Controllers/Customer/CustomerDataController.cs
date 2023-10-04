using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Services.CustomerService;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controller
{
  [ApiController]
  [Route("[controller]")]
  public class CustomerDataController : ControllerBase
  {
    private readonly ICustomerService _customerService;

    public CustomerDataController(ICustomerService customerService)
    {
      _customerService = customerService;
    }

    [HttpPost("order")]
    [Authorize]
    public async Task<IActionResult> PostOrder([FromBody] UserDataVM userData)
    {
      if (!ModelState.IsValid)
        return BadRequest("Please Provide Valid Field");
      var result = await _customerService.SetOrder(userData);
      if (result)
        return Ok("Order Set");
      else
        return BadRequest("couldn't set the Order");
    }
    [Authorize]
    [HttpGet("order")]
    public async Task<IActionResult> GetCustomerOrderData([FromBody] TokenVM tokenVM)
    {
      if (!ModelState.IsValid)
        return BadRequest("Please provide a Valid Field");
      try
      {
        var Data = await _customerService.GetCustomerOrderData(tokenVM);
        return Ok(Data);
      }
      catch (Exception e)
      {
        return BadRequest(e.ToString());
      }
    }

  }
}
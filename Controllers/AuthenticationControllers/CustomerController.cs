using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Authentication;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.Services.CustomerService;

namespace VirtualMenuAPI.Controller
{
  [ApiController]
  [Route("Auth/[controller]")]
  public class CustomerController : ControllerBase
  {
    private readonly ICustomerAuthService _customerAuthService;

    public CustomerController(ICustomerAuthService customerAuthService)
    {
      _customerAuthService = customerAuthService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> PostNewCustomer()
    {
      try
      {
        var result = await _customerAuthService.AddNewCustomer();
        return Ok(result);
      }
      catch (Exception e)
      {
        return BadRequest(e.ToString());
      }
    }
  }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Services.AuthServices;

namespace VirtualMenuAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthorController : ControllerBase
  {
    private readonly IAuthorService _authService;

    public AuthorController(IAuthorService authorService)
    {
      _authService = authorService;
    }

    [HttpPost("Add-Barista")]
    [Authorize(Roles = UserRoles.Manager)]
    public async Task<IActionResult> RegisterBarista(BaristaInfoIN baristaInfo)
    {
      if (!ModelState.IsValid)
        return BadRequest("Invalid Data");
      try
      {
        await _authService.RegisterBarista(baristaInfo);
        return Ok("New Barista Created");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
    //[Authorize(Roles = UserRoles.Manager)]

    [HttpPost("login-user")]
    public async Task<IActionResult> Login([FromBody] LoginIN loginVM)
    {
      if (!ModelState.IsValid)
        return BadRequest("please provide All required fields");
      try
      {
        var TokenValue = await _authService.Login(loginVM);
        return Ok(TokenValue);
      }
      catch (Exception ex)
      {
        return Unauthorized(ex.Message);
      }
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestIN tokenRequest)
    {
      if (!ModelState.IsValid)
        return BadRequest("please provide All required fields");
      try
      {
        var result = await _authService.RefreshToken(tokenRequest);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet("new-customer")]
    public async Task<IActionResult> AddNewCustomer()
    {
      var result = await _authService.AddNewCustomer();
      return Ok(result);
    }

  }
}
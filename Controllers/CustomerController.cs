using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controller
{
  [ApiController]
  [Route("[controller]")]
  public class CustomerController : ControllerBase
  {
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    public CustomerController(TokenValidationParameters tokenValidationParameters,
                              IConfiguration configuration,
                              DataContext context)
    {
      _tokenValidationParameters = tokenValidationParameters;
      _configuration = configuration;
      _context = context;
    }
    [HttpPost("new-user")]
    public async Task<IActionResult> SetNewOrder([FromBody] Order order)
    {
      if (!ModelState.IsValid)
        return BadRequest("Please Provide Valid Field");

      var newCustomer = new Customer()
      {
        Identity = Guid.NewGuid(),
        DateCreated = DateTime.Now,
        Orders = new List<Order> { order }
      };
      var result = await _context.Customers.AddAsync(newCustomer);
      await _context.SaveChangesAsync();
      if (result is not null)
      {
        var JwtToken = await GenerateJWTTokenAsync(newCustomer.Identity);
        return Ok(JwtToken);
      }
      return BadRequest("Couldn't Set Order");
    }

    [HttpPost]
    public async Task<IActionResult> SetOrder([FromBody] SetOrderVM orderVM)
    {
      if (!ModelState.IsValid)
        return BadRequest("Please Provide Valid Field");
      var sample = new Order();
      var context = new ValidationContext(sample, serviceProvider: null, items: null);
      var validationResults = new List<ValidationResult>();
      try
      {

        bool isValid = Validator.TryValidateObject(orderVM.Order, context, validationResults, true);
      }
      catch
      {
        return BadRequest("bad model");

      }
      var TokenHandler = new JwtSecurityTokenHandler();
      try
      {
        var result = TokenHandler.ValidateToken(orderVM.Token, _tokenValidationParameters, out var validatedToken);
        var jwt = TokenHandler.ReadJwtToken(orderVM.Token);
        string identity = jwt.Claims.First(c => c.Type == "Identity").Value;
        Console.WriteLine(identity);
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Identity.ToString() == identity);
        if (customer == null)
          return BadRequest("user doesn't Exist");
        customer.Orders.Add(orderVM.Order);
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return Ok("Order set");
      }
      catch
      {
        return Unauthorized();
      }

    }
    private async Task<string> GenerateJWTTokenAsync(Guid customerIdentity)
    {
      var authClaims = new List<Claim>(){
        new Claim("Identity", customerIdentity.ToString()),
      };

      var authSingingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value!));
      var token = new JwtSecurityToken(
        issuer: _configuration.GetSection("JWT:Issuer").Value,
        expires: DateTime.UtcNow.AddMinutes(20),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSingingKey, SecurityAlgorithms.HmacSha256)
      );
      var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

      return jwtToken;
    }
  }
}
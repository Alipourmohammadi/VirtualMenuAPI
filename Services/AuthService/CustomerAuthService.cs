using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Authentication
{
  public class CustomerAuthService : ICustomerAuthService
  {
    private readonly UserManager<Customer> _customerManager;
    private readonly IConfiguration _configuration;

    public CustomerAuthService(UserManager<Customer> customerManager, IConfiguration configuration)
    {
      _customerManager = customerManager;
      _configuration = configuration;
    }
    public async Task<string> AddNewCustomer()
    {
      var newCustomer = new Customer()
      {
        Identity = Guid.NewGuid(),
        DateCreated = DateTime.Now,
      };
      var result = await _customerManager.CreateAsync(newCustomer);
      if (result is null)
        throw new Exception("couldn't create New User");
      var JwtToken = await GenerateJWTTokenAsync(newCustomer.Identity);
      return JwtToken;
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
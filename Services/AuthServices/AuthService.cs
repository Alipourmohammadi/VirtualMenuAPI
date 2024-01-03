using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Data.Models;
using VirtualMenuAPI.Data.Models.Users;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Services.AuthServices
{
  public class AuthorService : IAuthorService
  {
    private readonly UserManager<Account> _userManager;
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthorService(
      UserManager<Account> userManager,
      IConfiguration configuration,
      DataContext context,
      TokenValidationParameters tokenValidationParameters)
    {
      _userManager = userManager;
      _configuration = configuration;
      _context = context;
      _tokenValidationParameters = tokenValidationParameters;
    }
    public async Task RegisterBarista(BaristaInfoIN baristaInfo)
    {
      var userExists = await _userManager.FindByNameAsync(baristaInfo.UserName);
      if (userExists != null)
        throw new Exception("User Already Exists");
      var newUser = new Account()
      {
        UserName = baristaInfo.UserName
      };
      await _userManager.CreateAsync(newUser, baristaInfo.Password);
      await _userManager.AddToRoleAsync(newUser, UserRoles.Barista);
    }
    public async Task<AuthResultDto> Login(LoginIN loginVM)
    {
      var userExists = await _userManager.FindByNameAsync(loginVM.UserName);
      if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginVM.Password))
      {
        var TokenValue = await GenerateJWTTokenAsync(userExists, null);
        //var isCustomer = await _userManager.IsInRoleAsync(userExists, "Customer");

        return TokenValue;
      }
      throw new Exception("userName or Password is wrong");
    }
    public async Task<AuthResultDto> RefreshToken(TokenRequestIN tokenRequest)
    {
      try
      {
        var result = await VerifyAndGenerateTokenAsync(tokenRequest);
        return result;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
    public async Task<AuthResultDto> AddNewCustomer()
    {
      var guid = Guid.NewGuid();
      var newCustomer = new Customer() { Identity = guid, UserName = guid.ToString() };
      await _userManager.CreateAsync(newCustomer);
      await _userManager.AddToRoleAsync(newCustomer, UserRoles.Customer);
      var result = await GenerateCustomerJWTTokenAsync(newCustomer);
      return result;
    }

    private async Task<AuthResultDto> GenerateCustomerJWTTokenAsync(Customer customer)
    {
      var authClaims = new List<Claim>(){
        new Claim("Identity", customer.Identity.ToString()),
        new Claim(ClaimTypes.Role, UserRoles.Customer)
    };

      var authSingingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value!));
      var token = new JwtSecurityToken(
        issuer: _configuration.GetSection("JWT:Issuer").Value,
        expires: DateTime.UtcNow.AddMinutes(20),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSingingKey, SecurityAlgorithms.HmacSha256)
      );
      var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

      var refreshToken = new RefreshToken()
      {
        IsRevoked = false,
        UserId = customer.Id,
        DateAdded = DateTime.UtcNow,
        DateExpire = DateTime.UtcNow.AddMonths(6),
        Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
        User = customer,
      };
      await _context.RefreshTokens.AddAsync(refreshToken);
      await _context.SaveChangesAsync();

      var response = new AuthResultDto()
      {
        Token = jwtToken,
        RefreshToken = refreshToken.Token,
        ExpiresAt = token.ValidTo
      };
      return response;
    }

    private async Task<AuthResultDto> VerifyAndGenerateTokenAsync(TokenRequestIN tokenRequest)
    {
      var jwtTokenHandler = new JwtSecurityTokenHandler();
      var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
      if (storedToken is null)
        throw new Exception("Token does not exist");

      var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
      if (dbUser == null) throw new Exception("Token dose not exist");
      if (storedToken.IsRevoked) throw new Exception("Token is not valid");
      try
      {
        var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
        return await GenerateJWTTokenAsync(dbUser, storedToken);
      }
      catch (SecurityTokenExpiredException)
      {
        if (storedToken.DateExpire >= DateTime.UtcNow)
        {
          return await GenerateJWTTokenAsync(dbUser, storedToken);
        }
        else
        {
          return await GenerateJWTTokenAsync(dbUser, null);
        }
      }
    }

    private async Task<AuthResultDto> GenerateJWTTokenAsync(Account user, RefreshToken rToken)
    {
      List<Claim> authClaims = new();
      //Add userRole Claims
      var userRoles = await _userManager.GetRolesAsync(user);
      foreach (var userRole in userRoles)
      {
        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
      }
      var isCustomer = await _userManager.IsInRoleAsync(user, "Customer");
      if (isCustomer)
      {
        authClaims.Add(new Claim("Identity", user.UserName));
      }
      else
      {
        authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
      };

      var authSingingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value!));
      var token = new JwtSecurityToken(
        issuer: _configuration.GetSection("JWT:Issuer").Value,
        expires: DateTime.UtcNow.AddMinutes(20),
        claims: authClaims,
        signingCredentials: new SigningCredentials(authSingingKey, SecurityAlgorithms.HmacSha256)
      );
      var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
      if (rToken != null)
      {
        var rTokenResponse = new AuthResultDto()
        {
          Token = jwtToken,
          RefreshToken = rToken.Token,
          ExpiresAt = token.ValidTo
        };
        return rTokenResponse;
      }

      var prevToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.User == user);

      if (prevToken != null)
        prevToken.IsRevoked = true;

      var refreshToken = new RefreshToken()
      {
        IsRevoked = false,
        UserId = user.Id,
        DateAdded = DateTime.UtcNow,
        DateExpire = DateTime.UtcNow.AddMonths(6),
        Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
        User = user
      };

      await _context.RefreshTokens.AddAsync(refreshToken);
      await _context.SaveChangesAsync();

      var response = new AuthResultDto()
      {
        Token = jwtToken,
        RefreshToken = refreshToken.Token,
        ExpiresAt = token.ValidTo
      };
      return response;
    }

  }
}

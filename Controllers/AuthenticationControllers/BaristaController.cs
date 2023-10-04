// using Microsoft.AspNetCore.Mvc;

// namespace VirtualMenuAPI.Authentication
// {
//   [ApiController]
//   [Route("[controller]")]
//   public class BaristaController:ControllerBase
//   {
//     public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
//     {
//       if (!ModelState.IsValid)
//         return BadRequest("please provide All required fields");

//       var userExists = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
//       if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginVM.Password))
//       {
//         var TokenValue = await GenerateJWTTokenAsync(userExists, null);
//         return Ok(TokenValue);
//       }
//       return Unauthorized();
//     }
//     [HttpPost("refresh-token")]
//     public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVM tokenRequestVM)
//     {
//       if (!ModelState.IsValid)
//         return BadRequest("please provide All required fields");

//       var result = await VerifyAndGenerateTokenAsync(tokenRequestVM);
//       return Ok(result);
//     }

//     private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM)
//     {
//       var jwtTokenHandler = new JwtSecurityTokenHandler();
//       var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestVM.RefreshToken);
//       var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
//       try
//       {
//         var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequestVM.Token, _tokenValidationParameters, out var validatedToken);
//         return await GenerateJWTTokenAsync(dbUser, storedToken);
//       }
//       catch (SecurityTokenExpiredException)
//       {
//         if (storedToken.DateExpire >= DateTime.UtcNow)
//         {
//           return await GenerateJWTTokenAsync(dbUser, storedToken);
//         }
//         else
//         {
//           return await GenerateJWTTokenAsync(dbUser, null);
//         }
//       }
//     }

//     private async Task<AuthResultVM> GenerateJWTTokenAsync(ApplicationUser user, RefreshToken rToken)
//     {
//       var authClaims = new List<Claim>(){
//         new Claim(ClaimTypes.Name, user.UserName),
//         new Claim(ClaimTypes.NameIdentifier, user.Id),
//         new Claim(JwtRegisteredClaimNames.Email, user.Email),
//         new Claim(JwtRegisteredClaimNames.Sub, user.Email),
//         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//       };
//       //Add userRole Claims
//       var userRoles = await _userManager.GetRolesAsync(user);
//       foreach (var userRole in userRoles)
//       {
//         authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//       }
      
//       var authSingingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("JWT:Secret").Value!));
//       var token = new JwtSecurityToken(
//         issuer: _configuration.GetSection("JWT:Issuer").Value,
//         audience: _configuration.GetSection("JWT:Audience").Value,
//         expires: DateTime.UtcNow.AddMinutes(1),
//         claims: authClaims,
//         signingCredentials: new SigningCredentials(authSingingKey, SecurityAlgorithms.HmacSha256)
//       );
//       var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
//       if (rToken != null)
//       {
//         var rTokenResponse = new AuthResultVM()
//         {
//           Token = jwtToken,
//           RefreshToken = rToken.Token,
//           ExpiresAt = token.ValidTo
//         };
//         return rTokenResponse;
//       }
//       var refreshToken = new RefreshToken()
//       {
//         JwtId = token.Id,
//         IsRevoked = false,
//         UserId = user.Id,
//         DateAdded = DateTime.UtcNow,
//         DateExpire = DateTime.UtcNow.AddMonths(6),
//         Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
//       };
//       await _context.RefreshTokens.AddAsync(refreshToken);
//       await _context.SaveChangesAsync();

//       var response = new AuthResultVM()
//       {
//         Token = jwtToken,
//         RefreshToken = refreshToken.Token,
//         ExpiresAt = token.ValidTo
//       };
//       return response;
//     }
//   }
// }
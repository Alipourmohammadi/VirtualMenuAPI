using Microsoft.AspNetCore.Mvc;
using VirtualMenuAPI.Data.Dtos;
using VirtualMenuAPI.Data.Inputs;

namespace VirtualMenuAPI.Services.AuthServices
{
  public interface IAuthorService
  {
    Task<AuthResultDto> AddNewCustomer();
    Task<AuthResultDto> RefreshToken(TokenRequestIN tokenRequest);
    Task<AuthResultDto> Login( LoginIN loginVM);
    Task RegisterBarista(BaristaInfoIN baristainfo);

  }
}

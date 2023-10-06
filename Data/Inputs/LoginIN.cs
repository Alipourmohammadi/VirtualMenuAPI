using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Data.Inputs
{
  public class LoginIN
  {
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid Range")]
    public string UserName { get; set; }= string.Empty;
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid Range")]
    public string Password { get; set; }= string.Empty;
  }
}
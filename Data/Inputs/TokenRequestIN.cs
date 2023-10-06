using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Data.Inputs
{
  public class TokenRequestIN
  {
    [Required]
    [StringLength(600, MinimumLength =5, ErrorMessage = "Invalid Data")]
    public string Token { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength =5, ErrorMessage = "Invalid Data")]
    public string RefreshToken { get; set; } = string.Empty;
  }
}

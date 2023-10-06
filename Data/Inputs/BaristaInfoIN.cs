using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Data.Inputs
{
  public class BaristaInfoIN
  {
    [StringLength(50,MinimumLength =5,ErrorMessage ="Out Of Range")]
    public string UserName { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 5, ErrorMessage = "Out Of Range")]
    public string Password { get; set; } = string.Empty;
  }
}

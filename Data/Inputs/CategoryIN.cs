using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VirtualMenuAPI.Data.Inputs
{
  public class CategoryIN
  { 
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Value length is Out of Range")]
    public string Title { get; set; } = string.Empty;
  }
}

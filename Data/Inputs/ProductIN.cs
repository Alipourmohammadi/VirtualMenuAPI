using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Data.Inputs
{
  public class ProductIN
  {
    private const string _errorMessage = "Out Of Range";

    [StringLength(100, MinimumLength = 5, ErrorMessage = _errorMessage)]
    public string Title { get; set; } = string.Empty;

    [Range(0, 10_000, ErrorMessage = _errorMessage)]
    public int Duration { get; set; }

    [Range(0, 99_999_999, ErrorMessage = _errorMessage)]
    public int Price { get; set; }
    public int CategoryId { get; set; }
  }
}

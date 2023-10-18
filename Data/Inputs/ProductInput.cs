using System.ComponentModel.DataAnnotations;
using VirtualMenuAPI.Extensions;

namespace VirtualMenuAPI.Data.Inputs
{
  public class ProductInput: IValidatableObject
  {
    private const string _errorMessage = "Out Of Range";
    public IFormFile Image { get; set; }

    [StringLength(100, MinimumLength = 1, ErrorMessage = _errorMessage)]
    public string Title { get; set; } = string.Empty;

    [Range(0, 10_000, ErrorMessage = _errorMessage)]
    public int Duration { get; set; }

    [Range(0, 99_999_999, ErrorMessage = _errorMessage)]
    public int Price { get; set; }
    public int CategoryId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return Image.IsImageFile();
      yield return Image.CheckMaxSize(2_097_152);
    }
  }
}

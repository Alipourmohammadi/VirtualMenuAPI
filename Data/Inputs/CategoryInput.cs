using System.ComponentModel.DataAnnotations;
using VirtualMenuAPI.Extensions;

namespace VirtualMenuAPI.Data.Inputs
{
  public class CategoryInput : IValidatableObject
  {
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Invalid Title Name")]
    public string Title { get; set; } = string.Empty;
    public IFormFile Image { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return Image.IsImageFile();
      yield return Image.CheckMaxSize(2_097_152);
    }
  }
}

using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Extensions
{
  public static class FormFileExtensions
  {
    public static ValidationResult IsImageFile(this IFormFile formFile)
    {
      var allowedContentTypes = new[] { "image/jpeg", "image/png" };
      var isValid = allowedContentTypes.Contains(formFile.ContentType);
      if (!isValid)
        return new ValidationResult("File is not an Image");

      return null;

    }

    public static ValidationResult CheckMaxSize(this IFormFile formFile, int maxSize)
    {
     if(formFile.Length > maxSize) return new ValidationResult($"file should be smaller than {maxSize/1024}KB");

      return null;
    }
  }
}

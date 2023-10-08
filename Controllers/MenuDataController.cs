
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.ViewModels;

namespace VirtualMenuAPI.Controller
{
  [ApiController]
  [Route("[controller]")]
  public class MenuDataController : ControllerBase
  {
    private readonly DataContext _dataContext;
    private readonly string _assetsFolderPath;


    public MenuDataController(DataContext dataContext)
    {
      _dataContext = dataContext;
      _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

    }
    [HttpGet]
    public async Task<MenuDataDto> GetMenuData()
    {
      return new MenuDataDto
      (
        await _dataContext.Categories.ToListAsync(),
        await _dataContext.Products.ToListAsync()
      );
    }
    [HttpGet("{imageName}")]
    public IActionResult GetImage(string imageName)
    {
      try
      {
        var filePath = Path.Combine(_assetsFolderPath, imageName);
        if (!System.IO.File.Exists(filePath))
          return NotFound();
        
        var contentType = GetContentType(imageName);

        if (contentType == null)
          return StatusCode(415, "Unsupported Media Type");

        return PhysicalFile(filePath, contentType);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Internal server error: {ex.Message}");
      }
    }
    private string GetContentType(string fileName)
    {
      var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

      switch (fileExtension)
      {
        case ".jpg":
        case ".jpeg":
          return "image/jpeg";
        case ".png":
          return "image/png";
        default:
          return null;
      }
    }
  }
}
using Microsoft.EntityFrameworkCore;
using VirtualMenuAPI.Data;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Services.ManagerServices
{
  public class ManagerService : IManagerService
  {
    private readonly DataContext _dataContext;
    private readonly string _assetsFolderPath;
    public ManagerService(DataContext dataContext)
    {
      _dataContext = dataContext;
      _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");
    }
    public async Task<Product> AddNewProduct(ProductIN productIn, IFormFile file)
    {
      string imageString;
      if (!IsImageFile(file))
        throw new Exception("Only JPEG and PNG images are allowed.");
      try
      {
        if (!Directory.Exists(_assetsFolderPath))
          throw new Exception("Internal server error: assets doesn't Exist");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        imageString = fileName;
        var filePath = Path.Combine(_assetsFolderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Internal server error: {ex.Message}");
      }
      var newProduct = new Product()
      {
        Image = imageString,
        Title = productIn.Title,
        Duration = productIn.Duration,
        Price = productIn.Price,
        CategoryId = productIn.CategoryId,
      };
      await _dataContext.Products.AddAsync(newProduct);
      await _dataContext.SaveChangesAsync();
      return newProduct;
    }
    public async Task<Category> AddNewCategory(CategoryIN categoryIn, IFormFile file)
    {
      string imageString;
      if (!IsImageFile(file))
        throw new Exception("Only JPEG and PNG images are allowed.");
      try
      {
        if (!Directory.Exists(_assetsFolderPath))
          throw new Exception("Internal server error: assets doesn't Exist");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        imageString = fileName;
        var filePath = Path.Combine(_assetsFolderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Internal server error: {ex.Message}");
      }
      var newCategory = new Category()
      {
        Image = imageString,
        Title = categoryIn.Title,
      };
      await _dataContext.Categories.AddAsync(newCategory);
      await _dataContext.SaveChangesAsync();
      return newCategory;
    }
    public async Task RemoveProduct(int id)
    {
      var result = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == id);
      if (result == null)
        throw new Exception("Product doesn't Exist");
      _dataContext.Products.Remove(result);
      await _dataContext.SaveChangesAsync();
    }
    public async Task RemoveCategory(int id)
    {
      var result = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
      if (result == null)
        throw new Exception("Category doesn't Exist");
      _dataContext.Categories.Remove(result);
      await _dataContext.SaveChangesAsync();
    }
    private bool IsImageFile(IFormFile file)
    {
      var allowedContentTypes = new[] { "image/jpeg", "image/png" };
      return allowedContentTypes.Contains(file.ContentType);
    }
  }
}

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
    public async Task<Product> AddNewProduct(ProductInput productIn)
    {
      string imageString;
      try
      {
        if (!Directory.Exists(_assetsFolderPath))
          Directory.CreateDirectory(_assetsFolderPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(productIn.Image.FileName)}";
        imageString = fileName;
        var filePath = Path.Combine(_assetsFolderPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await productIn.Image.CopyToAsync(stream);
      }
      catch (Exception ex)
      {
        throw new Exception($"Internal server error: {ex.Message}");
      }
      var theCategory = await _dataContext.Categories.FirstOrDefaultAsync(x=>x.Id == productIn.CategoryId);
      if (theCategory is null)
        throw new Exception($"The category :{productIn.CategoryId} dose not Exist");
      var newProduct = new Product()
      {
        Image = imageString,
        Title = productIn.Title,
        Duration = productIn.Duration,
        Price = productIn.Price,
        CategoryId = productIn.CategoryId,
        Category = theCategory
      };
      await _dataContext.Products.AddAsync(newProduct);
      await _dataContext.SaveChangesAsync();
      return newProduct;
    }
    public async Task<Category> AddNewCategory(CategoryInput categoryInput)
    {
      string imageString;
      try
      {
        if (!Directory.Exists(_assetsFolderPath))
          Directory.CreateDirectory(_assetsFolderPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(categoryInput.Image.FileName)}";
        imageString = fileName;
        var filePath = Path.Combine(_assetsFolderPath, fileName);


        using var stream = new FileStream(filePath, FileMode.Create);
        await categoryInput.Image.CopyToAsync(stream);
      }
      catch (Exception ex)
      {
        throw new Exception($"Internal server error: {ex.Message}");
      }
      var newCategory = new Category()
      {
        Image = imageString,
        Title = categoryInput.Title,
      };
      await _dataContext.Categories.AddAsync(newCategory);
      await _dataContext.SaveChangesAsync();
      return newCategory;
    }
    public async Task RemoveProduct(int id)
    {
      var result = await _dataContext.Products.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Product doesn't Exist");
      var filePath = Path.Combine(_assetsFolderPath, result.Image);
      if (!File.Exists(filePath))
        throw new Exception("file doesn't Exist");
      File.SetAttributes(filePath, FileAttributes.Normal);
      File.Delete(filePath);
      _dataContext.Products.Remove(result);
      await _dataContext.SaveChangesAsync();
    }
    public async Task RemoveCategory(int id)
    {
      var result = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Category doesn't Exist");
      var filePath = Path.Combine(_assetsFolderPath, result.Image);
      if (!File.Exists(filePath))
        throw new Exception("file doesn't Exist");
      File.SetAttributes(filePath, FileAttributes.Normal);
      File.Delete(filePath);

      _dataContext.Categories.Remove(result);
      await _dataContext.SaveChangesAsync();
    }
  }
}

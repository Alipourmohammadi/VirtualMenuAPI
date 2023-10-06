using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Services.ManagerServices
{
  public interface IManagerService
  {
    Task<Product> AddNewProduct(ProductIN product,IFormFile file);
    Task<Category> AddNewCategory(CategoryIN product, IFormFile file);
    Task RemoveProduct(int id);
    Task RemoveCategory(int id);

  }
}

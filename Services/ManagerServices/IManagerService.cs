using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Services.ManagerServices
{
  public interface IManagerService
  {
    Task<Product> UpdateProduct(int id, ProductInput productIn);
    Task<Product> AddNewProduct(ProductInput product);
    Task<Category> AddNewCategory(CategoryInput product);
    Task RemoveProduct(int id);
    Task RemoveCategory(int id);

  }
}

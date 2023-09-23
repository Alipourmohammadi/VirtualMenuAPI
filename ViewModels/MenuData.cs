using VirtualMenuAPI.Models;
using VirtualMenuAPI.Repository;

namespace VirtualMenuAPI.ViewModels
{
  public class MenuData
  {
    private CategoryRepository _categoryRepo;
    private ProductsRepository _productRepo;
    public List<Category> CategoryItems { get; set; } = new();
    public List<Product> ProductItems { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public MenuData()
    {
      _categoryRepo = new CategoryRepository();
      _productRepo = new ProductsRepository();
    }
    public MenuData GetMenuData(IOrderRepository _orderRepo)
    {
      return new MenuData
      {
        CategoryItems = _categoryRepo.GetAllCategories(),
        ProductItems = _productRepo.GetAllProducts(),
        Orders = _orderRepo.GetAllOrders()
      };
    }
  }
}
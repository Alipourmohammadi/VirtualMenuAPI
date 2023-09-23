using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Repository
{
  class CategoryRepository
  {
    public List<Category> Categories;
    public CategoryRepository()
    {
      Categories = new(){
        new(id:0,image:"pizza.jpg",title:"پیتزا"),
        new(id:1,image:"pizza.jpg",title:"برگر"),
        new(id:2,image:"pizza.jpg",title:"نوشیدنی")
      };
    }
    public List<Category> GetAllCategories()
    {
      return Categories;
    }
    public Category? GetCategoryById(int id)
    {
      return Categories.Find(x => x.Id == id);
    }
  }
}
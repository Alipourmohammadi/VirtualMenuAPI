
using VirtualMenuAPI.Models;

namespace VirtualMenuAPI.Repository
{
  class ProductsRepository
  {
    public List<Product> Products;

    public ProductsRepository()
    {
      Products = new(){
      new(id:0,image:"pepperoni-pizza.jpg",title:"پیتزا پپرونی",duration:20,price:75_000,categoryId:0),
      new(id:1,image:"special-pizza.jpg",title:"پیتزا اسپشیال",duration:15,price:86_000,categoryId:0),
      new(id:2,image:"chicken-pizza.jpg",title:"پیتزا مرغ",duration:10,price:93_000,categoryId:0),
      new(id:3,image:"vegan-pizza.jpg",title:"پیتزا سبزیجات",duration:15,price:67_000,categoryId:0),
      new(id:4,image:"sandwich.jpg",title:"همبرگر",duration:5,price:48_000,categoryId:1),
      new(id:5,image:"tea.jpg",title:"چای ماسالا",duration:3,price:25_000,categoryId:2),
    };
    }
    public List<Product> GetAllProducts()
    {
      return Products;
    }
    public Product? GetProductById(int id)
    {
      return Products.Find(x=>x.Id==id);
    }
    public List<Product> GetProductsByCategory(int id)
    {
      return Products.Where(x=>x.CategoryId==id).ToList();
    }

  }
}
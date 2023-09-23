using System.ComponentModel.DataAnnotations;

namespace VirtualMenuAPI.Models
{
  public class Category
  {
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public List<Product> Products {get; private set; }
    public Category(int id, string title, string image)
    {
      Id = id;
      Title = title;
      Image = image;
    }
  }
}
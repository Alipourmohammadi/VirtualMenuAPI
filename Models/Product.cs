namespace VirtualMenuAPI.Models
{
  public class Product
  {
    public int Id { get; private set; }
    public string Image { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public int Duration { get; private set; }
    public int Price { get; private set; }
    public int CategoryId { get; private set; }

    public Product(int id, string image, string title, int duration, int price, int categoryId)
    {
      Id = id;
      Image = image;
      Title = title;
      Duration = duration;
      Price = price;
      CategoryId = categoryId;
    }
  }
}
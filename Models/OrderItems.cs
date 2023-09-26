namespace VirtualMenuAPI.Models
{
  public class OrderItems
  {
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public int Count { get; set; }

    public OrderItems(int id, string title,int count)
    {
      Id = id;
      Title = title;
      Count = count;
    }
  }
}
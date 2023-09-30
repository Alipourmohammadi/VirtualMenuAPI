namespace VirtualMenuAPI.Models
{
  public enum ServeType
  {
    Present,
    TakAway
  }
  public class Order
  {
    public int Id { get; set; }
    public DateTime SubmitDate { get; set; } = DateTime.Now;
    public int TableNumber { get; set; }
    public ServeType ServeType { get; set; } = ServeType.Present;
    public List<OrderItem> Items { get; set; } = new();
  }
}
namespace VirtualMenuAPI.Models{
  public enum ServeType{
    Present,
    TakAway
  }
  public class Order
  {
    public int? Id { get; private set; }
    public DateTime SubmitDate { get; private set; }
    public int TableNumber { get; private set; } = 2;
    public ServeType ServeType { get; private set; } = ServeType.Present;
    public List<OrderItems> Items { get;  set; } = new();
  }
}
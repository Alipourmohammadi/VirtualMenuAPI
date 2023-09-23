namespace VirtualMenuAPI.Models{
  public enum ServeType{
    Present,
    TakAway
  }
  public class Order
  {
    public int Id { get; set; }
    public DateTime SubmitDate { get; private set; }
    public int? TableNumber { get; private set; }
    public ServeType ServeType { get; private set; } = ServeType.Present;
    public List<Product> Items { get;  set; } = new();
  }
}
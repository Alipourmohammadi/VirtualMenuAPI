namespace VirtualMenuAPI.Data.Events
{
  public abstract class SseEvent
  {
    public Guid Id { get; set; }
    public abstract string GetContent();
  }
}
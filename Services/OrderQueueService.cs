using System.Collections.Concurrent;
using VirtualMenuAPI.Dto;

namespace VirtualMenuAPI.Services
{

  public interface IOrderQueueService
  {
    void EnqueueMessage(CustomerOrderDto message);
    bool TryDequeueMessage(out CustomerOrderDto message);
  }

  public class OrderQueueService : IOrderQueueService
  {
    private readonly ConcurrentQueue<CustomerOrderDto> _messageQueue = new ();

    public void EnqueueMessage(CustomerOrderDto message)
    {
      _messageQueue.Enqueue(message);
    }

    public bool TryDequeueMessage(out CustomerOrderDto message)
    {
      return _messageQueue.TryDequeue(out message);
    }
  }
}

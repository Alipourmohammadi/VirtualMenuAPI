// using System.Net.Http.Headers;
// using VirtualMenuAPI.Models;

// namespace VirtualMenuAPI.Repository;

// public interface IOrderRepository
// {
//   List<Order> SetOrder(Order order);
//   List<Order> GetAllOrders();
//   void RemoveOrder(Order order);
// }

// public class OrderRepository : IOrderRepository
// {
//   private List<Order> _orders;
//   public OrderRepository()
//   {
//     _orders = new();
//   }

//   public List<Order> SetOrder(Order order)
//   {
//     // if (order != null)
//     // {
//     _orders.Add(order);
//     return GetAllOrders();
//     // }
//     // else return false;
//   }
//   public List<Order> GetAllOrders()
//   {
//     return _orders;
//   }
//   public void RemoveOrder(Order order)
//   {
//     _orders.Remove(order);//?
//   }
// }
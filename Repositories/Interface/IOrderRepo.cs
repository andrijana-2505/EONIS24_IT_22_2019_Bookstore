using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IOrderRepo
    {
        void Create(Order order);
        int CreateId(Order order);
        void Update(Order order);
        Task Delete(int ordersId);
        Order FindOrderById(int ordersId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Order GetOrderInProgressForUser(int userId);
        IEnumerable<Order> GetOrders();
        IEnumerable<Orderitem> GetOrderItemsForOrder(int orderId);
        bool SaveChanges();
        IEnumerable<Address> GetAddressForOrder(int orderId);
    }
}

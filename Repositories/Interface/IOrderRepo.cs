using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IOrderRepo
    {
        void Create(Order order);
        void Update(Order order);
        void Delete(int ordersId);
        Order FindOrderById(int ordersId);
        Order GetOrderInProgressForUser(int userId);
        IEnumerable<Order> GetOrders();
        IEnumerable<Orderitem> GetOrderItemsForOrder(int orderId);
        bool SaveChanges();
    }
}

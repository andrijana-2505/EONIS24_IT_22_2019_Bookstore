using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IOrderItemRepo
    {
        void Create(Orderitem orderitem);
        void Update(Orderitem orderitem);
        void Delete(int orderitemId);
        Orderitem FindOrderItemById(int orderitemId);
        IEnumerable<Orderitem> GetOrderItems(int? ordersId);
        bool SaveChanges();

        IEnumerable<Book> GetBookForOrderItem(int orderitemId);
        IEnumerable<Order> GetOrderForOrderItem(int orderitemId);
    }
}

using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;

namespace BackendBookstore.Repositories.Implementation
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly PostgresContext _context;
        private readonly IBookRepo _repo;


        public OrderItemRepo(PostgresContext context, IBookRepo repo)
        {
            _context = context;
            _repo = repo;


        }

        public void Create(Orderitem orderitem)
        {
            if (orderitem == null)
                return;
            if (orderitem.Quantity > _repo.FindBookById((int)orderitem.BookId).Available)
                return;
            _context.Orderitems.Add(orderitem);
        }

        public void Delete(int orderitemId)
        {
            var orderitem = FindOrderItemById(orderitemId);
            if (orderitem != null)
            {
                _context.Orderitems.Remove(orderitem);
            }
        }

        public Orderitem FindOrderItemById(int orderitemId)
        {
            var orderitem = _context.Orderitems.FirstOrDefault(o => o.OrderItemId == orderitemId);
            if (orderitem != null)
            {
                return orderitem;
            }
            else
            {
                throw new ArgumentNullException($"Order item with ID {orderitemId} not found.", nameof(orderitemId));
            }
        }
        public void Update(Orderitem orderitem)
        {

        }

        public IEnumerable<Orderitem> GetOrderItems(int? ordersId)
        {
            return _context.Orderitems.Where(o => (ordersId == null || o.OrdersId == ordersId)).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;

namespace BackendBookstore.Repositories.Implementation
{
    public class OrderRepo : IOrderRepo
    {
        private readonly PostgresContext _context;

        public OrderRepo(PostgresContext context)
        {
            _context = context;
        }

        public void Create(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _context.Orders.Add(order);
        }
        public int CreateId(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _context.Orders.Add(order);
            _context.SaveChanges(); // Sačuvaj promene u bazi

            return order.OrdersId; // Vrati generisani ID
        }


        public async Task Delete(int ordersId)
        {
            var order = FindOrderById(ordersId);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
        }

        public Order FindOrderById(int ordersId)
        {
            var orders = _context.Orders.FirstOrDefault(o => o.OrdersId == ordersId);
            if (orders != null)
                return orders;
            else
                throw new ArgumentException($"Order with ID {ordersId} not found.", nameof(ordersId));
        }

        public void Update(Order order)
        {
        }

        public Order GetOrderInProgressForUser(int userId)
        {
            return _context.Orders.FirstOrDefault(o => o.UsersId == userId && o.Status == OrderStatus.U_procesu);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }

        public IEnumerable<Orderitem> GetOrderItemsForOrder(int orderId)
        {
            return _context.Orderitems.Where(o => o.OrdersId == orderId).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Address> GetAddressForOrder(int orderId)
        {
            return _context.Addresses.Where(a => a.OrdersId == orderId).ToList();
        }
    }
}

using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;

namespace BackendBookstore.Repositories.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly PostgresContext _context;
        private readonly IAddressRepo _repo;


        public UserRepo(PostgresContext context, IAddressRepo repo)
        {
            _context = context;
            _repo = repo;
        }
        public void Create(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            _context.Users.Add(user);

        }

        public void Delete(int userId)
        {
            var user = FindUserById(userId);
            _context.Users.Remove(user);
        }

        public User FindByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
           
            return user;

        }

        public User FindUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UsersId == userId);
            if (user != null)
                return user;
            else
                throw new ArgumentException($"User with ID {userId} not found.", nameof(userId));
        }

        public void Update(User user)
        {

        }

        public IEnumerable<User> GetUsers(UserRole? userRole)
        {
            if (userRole == null)
            {
                // Ako je UserRole null, vrati sve korisnike
                return _context.Users.ToList();
            }
            else
            {
                // Inače, konvertuj enum u string i uporedi sa vrednostima u bazi
                string roleString = userRole.ToString();
                return _context.Users.Where(u => u.UserRole.ToString() == roleString).ToList();
            }
        }
        public IEnumerable<Order> GetOrdersForUser(int usersId)
        {
            var orders = _context.Orders
        .Where(u => u.UsersId == usersId && u.Status != OrderStatus.U_procesu)
        .ToList();

            return orders;

        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Review> GetReviewsForUser(int usersId)
        {
            return _context.Reviews.Where(u => u.UsersId == usersId).ToList();
        }
    }
}

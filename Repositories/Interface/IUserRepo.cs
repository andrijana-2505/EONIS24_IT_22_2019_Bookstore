using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IUserRepo
    {
        void Create(User user);
        void Update(User user);
        void Delete(int userId);
        User FindUserById(int userId);
        User FindByEmail(string email);
        IEnumerable<User> GetUsers(UserRole? userRole);
        IEnumerable<Order> GetOrdersForUser(int usersId);
        //IEnumerable<Address> GetAddresses(int usersId);
        IEnumerable<Review> GetReviewsForUser(int usersId);
        bool SaveChanges();
    }
}

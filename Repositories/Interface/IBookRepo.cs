using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IBookRepo
    {
        void Create(Book book);
        void Update(Book book);
        void Delete(int bookId);
        Book FindBookById(int bookId);
        IEnumerable<Book> GetBooks(int? categoryId, string? search);
        IEnumerable<Orderitem> GetOrderItemsForBook(int bookId);
        IEnumerable<Book> GetBooksByIds(List<int> bookIds);
        IEnumerable<Review> GetReviewsForBook(int bookId);
        bool SaveChanges();
    }
}

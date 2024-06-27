using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackendBookstore.Repositories.Implementation
{
    public class BookRepo : IBookRepo
    {
        private readonly PostgresContext _context;
        public BookRepo(PostgresContext context)
        {
            _context = context;
        }
        public void Create(Book book)
        {
            if (book != null)
                _context.Books.Add(book);
        }

        public void Delete(int bookId)
        {
            var book = FindBookById(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
        }

        public Book FindBookById(int bookId)
        {
            var book = _context.Books
            .Include(b => b.Orderitems)
            .Include(b => b.Reviews)
            .Include(b => b.Category)
            .FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
                return book;
            else
                throw new ArgumentException($"Book with ID {bookId} not found.", nameof(bookId));
        }

        public void Update(Book book)
        {

        }

        public IEnumerable<Book> GetBooks(int? categoryId, string? search)
        {
            var query = _context.Books.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.BookTitle.Contains(search) ||
                                          b.BookAuthor.Contains(search) ||
                                          b.Publisher.Contains(search));
            }

            return query.ToList();
        }
        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }
        public IEnumerable<Orderitem> GetOrderItemsForBook(int bookId)
        {
            return _context.Orderitems.Where(b => b.BookId == bookId).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Book> GetBooksByIds(List<int> bookIds)
        {
            var books = _context.Books.Where(b => bookIds.Contains(b.BookId)).ToList();
            return books;
        }

        public IEnumerable<Review> GetReviewsForBook(int bookId)
        {
            return _context.Reviews.Where(b => b.BookId == bookId).ToList();
        }
    }
}

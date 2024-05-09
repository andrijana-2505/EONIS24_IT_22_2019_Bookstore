using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;

namespace BackendBookstore.Repositories.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly PostgresContext _context;
        public CategoryRepo(PostgresContext context)
        {
            _context = context;
        }
        public void Create(Category category)
        {
            if (category == null)
                _context.Categories.Add(category);
        }

        public void Delete(int categoryId)
        {
            var category = FindCategoryById(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

        }

        public Category FindCategoryById(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
                return category;
            else
                throw new ArgumentException($"Category with ID {categoryId} not found.", nameof(categoryId));
        }

        public void Update(Category category)
        {

        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Book> GetBooksForCategory(int categoryId)
        {
            return _context.Books.Where(c => c.CategoryId == categoryId).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

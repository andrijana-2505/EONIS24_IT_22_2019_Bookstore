using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface ICategoryRepo
    {
        void Create(Category category);
        void Update(Category category);
        void Delete(int categoryId);
        Category FindCategoryById(int categoryId);
        IEnumerable<Category> GetCategories();
        IEnumerable<Book> GetBooksForCategory(int categoryId);
        bool SaveChanges();
    }
}

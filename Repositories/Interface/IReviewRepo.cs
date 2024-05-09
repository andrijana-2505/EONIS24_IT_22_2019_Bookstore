using BackendBookstore.Models;

namespace BackendBookstore.Repositories.Interface
{
    public interface IReviewRepo
    {
        void Create(Review review);
        void Delete(int reviewId);
        Review FindReviewById(int reviewId);
        IEnumerable<Review> GetReviews(int? usersId);
        bool SaveChanges();

    }
}

using BackendBookstore.Models;
using BackendBookstore.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackendBookstore.Repositories.Implementation
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly PostgresContext _context;

        public ReviewRepo(PostgresContext context)
        {
            _context = context;
        }
        public void Create(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            _context.Reviews.Add(review);
        }

        public void Delete(int reviewId)
        {
            var review = FindReviewById(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);

            }
        }


        public Review FindReviewById(int reviewId)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);
            if (review != null)
                return review;
            else
                throw new ArgumentException($"Review with ID {reviewId} not found.", nameof(reviewId));
        }

        public IEnumerable<Book> GetBookForReview(int reviewId)
        {
            var review = _context.Reviews
                .Include(r => r.Book)  // Ovo uključuje povezanu knjigu
                .FirstOrDefault(r => r.ReviewId == reviewId);

            if (review?.Book != null)
            {
                return new List<Book> { review.Book };
            }

            return new List<Book>();  // Vratite praznu listu ako recenzija ili knjiga ne postoji
        }

        public IEnumerable<Review> GetReviews(int? usersId)
        {
            return _context.Reviews.Where(u => (usersId == null || u.UsersId == usersId));
        }

        public IEnumerable<User> GetUserForReview(int reviewId)
        {
            var review = _context.Reviews
                .Include(r => r.Users)  // Ovo uključuje povezanu knjigu
                .FirstOrDefault(r => r.ReviewId == reviewId);

            if (review?.Users != null)
            {
                return new List<User> { review.Users };
            }

            return new List<User>();  // Vratite praznu listu ako recenzija ili knjiga ne postoji
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }


    }
}

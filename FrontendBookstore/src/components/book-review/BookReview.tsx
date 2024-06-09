import './BookReview.css'
import Review from "../../model/Review.ts";

interface ReviewProps {
  review: Review
}

const BookReview = ({review}: ReviewProps) => {
  return <div className="review-component">
    <h4>Date: {review.reviewDate ? new Date(review.reviewDate).toLocaleString() : 'No date provided'}</h4>
    <p>User: {review.usersId}</p>
    <p>Rating: {review.rating}</p>
  </div>
}

export default BookReview;
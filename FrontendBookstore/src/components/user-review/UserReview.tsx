import './UserReview.css'
import {ReviewUpdateDto} from "../../dto/review/ReviewUpdateDto.ts";

interface ReviewProps {
  review: ReviewUpdateDto
}

const UserReview = ({review}: ReviewProps) => {
  return <div className="review-component">
    <h4>Date: {review.reviewDate ? new Date(review.reviewDate).toLocaleString() : 'No date provided'}</h4>
    <p>User: {review.usersId}</p>
    <p>Rating: {review.rating}</p>
  </div>
}

export default UserReview;
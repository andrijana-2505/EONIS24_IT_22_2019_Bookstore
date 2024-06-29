import UserReview from '../../components/user-review/UserReview.tsx';
import {UserReadDto} from '../../dto/user/UserReadDto';
import useAuth from '../../hooks/useAuth';
import OrderStatus from '../../model/OrderStatus.ts';
import './Profile.css';


const Profile = () => {
  const { userData } = useAuth();

  // Assume userData is of type UserReadDto and is not null
  const user: UserReadDto = userData!;

  const getOrderStatusString = (status: OrderStatus) => {
    switch (status) {
        case OrderStatus.Obrada:
            return "Obrada";
        case OrderStatus.Isporuka:
            return "Isporuka";
        case OrderStatus.Završeno:
            return "Završeno";
        case OrderStatus.Odbijeno:
            return "Odbijeno";
        case OrderStatus.U_procesu:
            return "U_procesu";

    }
};
  return (
    <div className='profile'>
      <div className='profile__profile-card'>
        <div className='profile__profile-card__profile-info'>
          <h2>Profile Information</h2>
          <span>
            <strong>Email:</strong> {user.email}
          </span>
          <span>
            <strong>Username:</strong> {user.username}
          </span>
          <span>
            <strong>First Name:</strong> {user.firstName}
          </span>
          <span>
            <strong>Last Name:</strong> {user.lastName}
          </span>
          <span>
            <strong>Phone:</strong> {user.phone}
          </span>
          <span>
            <strong>Genre:</strong> {user.genre}
          </span>
        </div>
      </div>
      <div className='profile__user-data'>
        <div className='profile__user-data_orders'>
          <h3>Orders</h3>
          {user.orders && user.orders.length > 0 ? (
            user.orders.map((order) => (
              <div key={order.ordersId} className="order-details">
                <p>Order ID: {order.ordersId}</p>
                <p>Total Amount: {order.totalAmount}</p>
                <p>Status: {getOrderStatusString(order.status as OrderStatus)}</p>
              </div>
            ))
          ) : (
            <p>No orders</p>
          )}
        </div>
        <div className='profile__user-data_review'>
          <h3>Reviews</h3>
          {user.reviews && user.reviews.length > 0 ? (
            user.reviews.map((review) => (
              <UserReview key={review.reviewId} review={review} />
            ))
          ) : (
            <p>No reviews</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;

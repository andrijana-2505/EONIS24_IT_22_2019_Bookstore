import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Order from '../../components/order/Order';
import UserReview from '../../components/user-review/UserReview.tsx';
import {UserReadDto} from '../../dto/user/UserReadDto';
import useAuth from '../../hooks/useAuth';
import './Profile.css';

// TODO backend issue -> user not getting reviews and orders
const Profile = () => {
  const { userData, userToken } = useAuth();
  const [userDetails, setUserDetails] = useState<UserReadDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const baseUrl = import.meta.env.VITE_BASE_URL;

  useEffect(() => {
    const fetchUserDetails = async () => {
      setError(''); // Clear previous error
      setLoading(true);
      try {
        const response = await axios.get(`${baseUrl}/api/User/details`, {
          headers: {
            Authorization: `Bearer ${userToken?.token}`,
          },
        });

        setUserDetails(response.data);
        console.log(response.data); // Check the data structure
        setLoading(false);
      } catch (error) {
        setError(error.response?.data?.message || 'Failed to fetch user details');
        setLoading(false);
      }
    };

    fetchUserDetails();
  }, [baseUrl, userToken]);

  if (loading) {
    return <div className='loading-message'>Loading....</div>;
  }

  if (error) {
    return <div className='error-message'>{error}</div>;
  }

  if (!userDetails) {
    return <div className='error-message'>Failed to load user details</div>;
  }
  return (
    <div className='profile'>
      <div className='profile__profile-card'>
        <div className='profile__profile-card__profile-info'>
          <h2>Profile Information</h2>
          <span>
            <strong>Email:</strong> {userDetails.email}
          </span>
          <span>
            <strong>Username:</strong> {userDetails.username}
          </span>
          <span>
            <strong>First Name:</strong> {userDetails.firstName}
          </span>
          <span>
            <strong>Last Name:</strong> {userDetails.lastName}
          </span>
          <span>
            <strong>Phone:</strong> {userDetails.phone}
          </span>
          <span>
            <strong>Genre:</strong> {userDetails.genre}
          </span>
        </div>
      </div>
      <div className='profile__user-data'>
        <div className='profile__user-data_orders'>
          <h3>Orders</h3>
          {userDetails.orders && userDetails.orders.length > 0 ? (
            userDetails.orders.map((order) => (
              <Order key={order.ordersId} order={order} />
            ))
          ) : (
            <p>No orders</p>
          )}
        </div>
        <div className='profile__user-data_review'>
          <h3>Reviews</h3>
          {userDetails.reviews && userDetails.reviews.length > 0 ? (
            userDetails.reviews.map((review) => (
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
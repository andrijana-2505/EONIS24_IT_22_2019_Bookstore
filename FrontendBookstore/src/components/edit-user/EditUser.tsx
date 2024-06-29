import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, Link, useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import './EditUser.css';
import { UserUpdateDto } from '../../dto/user/UserUpdateDto';
import UserRole from '../../model/UserRole';

const EditUser = () => {
  const { userId } = useParams();
  const { userToken } = useAuth();
  const navigate = useNavigate();

  const [user, setUser] = useState<UserUpdateDto>({
    usersId: parseInt(userId!),
    userRole: UserRole.Customer,
    username: '',
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    genre: ''
  });

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await axios.get(`http://localhost:5137/api/User/id/${userId}`, {
          headers: {
            Authorization: `Bearer ${userToken?.token}`,
          },
        });
        const fetchedUser = response.data;

        setUser({
          usersId: fetchedUser.usersId,
          username: fetchedUser.username,
          firstName: fetchedUser.firstName,
          lastName: fetchedUser.lastName,
          email: fetchedUser.email,
          phone: fetchedUser.phone,
          userRole: fetchedUser.userRole,
        });
      } catch (error) {
        console.error('Error fetching user:', error);
      }
    };

    fetchUser();
  }, [userId]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setUser({ ...user, [name]: value });
  };

  const handleRoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedRole = parseInt(e.target.value);
    setUser({ ...user, userRole: selectedRole });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.put('http://localhost:5137/api/User', user, {
        headers: {
          Authorization: `Bearer ${userToken?.token}`,
        },
      });
      console.log('User updated successfully');
      navigate('/admin-dashboard/users-list');
      // Redirect or display success message
    } catch (error) {
      console.error('Error updating user:', error);
      // Handle update error
    }
  };

  return (
    <div className="edit-user-form">
      <h2>Edit User</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="usersId">User ID:</label>
          <input
            type="text"
            id="usersId"
            name="usersId"
            value={user.usersId}
            readOnly
          />
        </div>
        <div className="form-group">
          <label htmlFor="username">Username:</label>
          <input
            type="text"
            id="username"
            name="username"
            value={user.username}
            readOnly
          />
        </div>
        <div className="form-group">
          <label htmlFor="firstName">First Name:</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={user.firstName}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="lastName">Last Name:</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={user.lastName}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            name="email"
            value={user.email}
            readOnly
          />
        </div>
        <div className="form-group">
          <label htmlFor="phone">Phone:</label>
          <input
            type="text"
            id="phone"
            name="phone"
            value={user.phone}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="userRole">User Role:</label>
          <select
            id="userRole"
            name="userRole"
            value={user.userRole}
            onChange={handleRoleChange}
          >
            <option value={0}>Customer</option>
            <option value={1}>Admin</option>
          </select>
        </div>
        <button type="submit">Update</button>
      </form>
      <div className="form-group">
        <Link to="/admin-dashboard/users-list" className="back-link">Back to User List</Link>
      </div>
    </div>
  );
};

export default EditUser;

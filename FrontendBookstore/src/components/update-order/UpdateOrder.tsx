import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import OrderStatus from '../../model/OrderStatus';
import './UpdateOrder.css';
import { OrderUpdateDto } from '../../dto/order/OrderUpdateDto';

const UpdateOrder = () => {
  const { orderId } = useParams<{ orderId: string }>();
  const { userToken } = useAuth();
  const navigate = useNavigate();

  const [order, setOrder] = useState({
    ordersId: parseInt(orderId!),
    totalAmount: 0,
    status: 0,
    orderDate: '',
    stripeTransactionId: '',
    usersId: 0
  });

  const fetchOrder = async () => {
    try {
      const response = await axios.get(`http://localhost:5137/api/Order/${orderId}`, {
        headers: {
          Authorization: `Bearer ${userToken?.token}`,
        },
      });
      const fetchedOrder = response.data;

      setOrder({
        ordersId: fetchedOrder.ordersId,
        totalAmount: fetchedOrder.totalAmount,
        status: fetchedOrder.status,
        orderDate: new Date(fetchedOrder.orderDate).toISOString().split('T')[0],
        stripeTransactionId: fetchedOrder.stripeTransactionId,
        usersId: fetchedOrder.usersId,
      });
    } catch (error) {
      console.error('Error fetching order:', error);
    }
  };

  useEffect(() => {
    fetchOrder();
  }, [orderId]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setOrder({ ...order, [name]: value });
  };

  const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedStatus = parseInt(e.target.value);
    setOrder({ ...order, status: selectedStatus });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.put(`http://localhost:5137/api/Order`, order, {
        headers: {
          Authorization: `Bearer ${userToken?.token}`,
        },
      });
      console.log('Order updated successfully');
      navigate('/admin-dashboard/order-history');
    } catch (error) {
      console.error('Error updating order:', error);
    }
};


  return (
    <div className="update-order-form">
      <h2>Update Order</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="ordersId">Order ID:</label>
          <input
            type="text"
            id="ordersId"
            name="ordersId"
            value={order.ordersId}
            readOnly
          />
        </div>
        <div className="form-group">
          <label htmlFor="usersId">User ID:</label>
          <input
            type="text"
            id="usersId"
            name="usersId"
            value={order.usersId}
            readOnly
          />
        </div>
        <div className="form-group">
          <label htmlFor="orderDate">Order Date:</label>
          <input
            type="date"
            id="orderDate"
            name="orderDate"
            value={order.orderDate}
            readOnly
          />
        </div>
        <div className="form-group">
          <label htmlFor="status">Order Status:</label>
          <select
            id="status"
            name="status"
            value={order.status}
            onChange={handleStatusChange}
          >
            <option value={0}>Obrada</option>
            <option value={1}>Isporuka</option>
            <option value={2}>Završeno</option>
            <option value={3}>Odbijeno</option>
            <option value={4}>U_procesu</option>
          </select>
        </div>
        <div className="form-group">
          <label htmlFor="totalAmount">Total Amount:</label>
          <input
            type="number"
            id="totalAmount"
            name="totalAmount"
            value={order.totalAmount}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="stripeTransactionId">Stripe Transaction ID:</label>
          <input
            type="text"
            id="stripeTransactionId"
            name="stripeTransactionId"
            value={order.stripeTransactionId}
            readOnly
          />
        </div>
        <button type="submit">Update</button>
      </form>
    </div>
  );
};

export default UpdateOrder;

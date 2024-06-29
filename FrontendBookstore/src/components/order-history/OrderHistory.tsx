import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import { OrderReadDto } from '../../dto/order/OrderReadDto';
import './OrderHistory.css'
import OrderStatus from '../../model/OrderStatus';

const OrderHistory = () => {
  const [orders, setOrders] = useState<OrderReadDto[]>([]);
  const { userToken } = useAuth();
  const navigate = useNavigate();


  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const headers = { Authorization: `Bearer ${userToken?.token}` };
        const response = await axios.get("http://localhost:5137/api/Order", { headers });
        setOrders(response.data);
      } catch (error) {
        console.error("Error fetching orders:", error);
      }
    };

    fetchOrders();
  }, []);
  const handleEditClick = (orderId: number) => {
    navigate(`/admin-dashboard/update-order/${orderId}`);
  };
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
const deleteOrder = (orderId: number) => {
  const confirmed = window.confirm("Da li ste sigurni da želite da obrišete porudzbinu?");

  if (confirmed) {

    axios
      .delete(`http://localhost:5137/api/Order/${orderId}`, {
        headers: {
          Authorization: `Bearer ${userToken?.token}`,
        },
      })
      .then((response) => {
        if (response.status === 204) {
          alert(`Porudzbina sa ID ${orderId} je obrisana`);
          setOrders(orders.filter((order) => order.ordersId !== orderId));
        }
      })
      .catch((error) => {
        console.error("Error deleting order:", error);
      });
  }
};

  return (
    <div className="order-history">
      <h2>Lista narudžbina</h2>
      <table>
        <thead>
          <tr>
            <th>ID narudžbine</th>
            <th>ID kupca</th>
            <th>Datum narudžbine</th>
            <th>Status narudžbine</th>
            <th>Ukupni iznos</th>
            <th>Stripe ID</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.ordersId}>
              <td>{order.ordersId}</td>
              <td>{order.usersId}</td>
              <td>{new Date(order.orderDate).toLocaleDateString()}</td>
              <td>{getOrderStatusString(order.status as OrderStatus)}</td>
              <td>{order.totalAmount}</td>
              <td>{order.stripeTransactionId}</td>
              <td>
                <button className="button-update" onClick={() => handleEditClick(order.ordersId)}>Izmeni</button>
              </td>
              <td>
                <button className="button-delete" onClick={() => deleteOrder(order.ordersId)}>Obriši</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default OrderHistory;
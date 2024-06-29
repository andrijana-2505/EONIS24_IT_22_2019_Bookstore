// src/models/Order.ts
import OrderStatus from './OrderStatus';
import Orderitem from './Orderitem';
import User from './User';
import Address from './Address';

interface Order {
  ordersId: number;
  totalAmount?: number;
  status: OrderStatus;
  orderDate?: string; // ili Date ako koristiš Date objekat u JavaScript-u/TypeScript-u
  stripeTransactionId?: string;
  usersId?: number;
  addresses: Address[];
  orderitems: Orderitem[];
  users?: User;
}

export default Order;

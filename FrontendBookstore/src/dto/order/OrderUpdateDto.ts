import OrderStatus from "../../model/OrderStatus";

export interface OrderUpdateDto {
  ordersId: number;
  totalAmount?: number;
  status?: OrderStatus;
  orderDate?: string;
  stripeTransactionId?: string;
  usersId?: number;
}
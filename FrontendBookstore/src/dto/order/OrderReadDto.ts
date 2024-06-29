import OrderStatus from "../../model/OrderStatus";

export interface OrderReadDto {
    ordersId: number;
    totalAmount: number;
    status: OrderStatus;
    orderDate: Date;
    stripeTransactionId: string;
    usersId: number;
  }
  
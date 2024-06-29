export interface OrderUpdateDto {
  ordersId?: number;
  totalAmount?: number;
  status?: string;
  orderData?: Date;
  stripeTransactionId?: string;
  usersId?: number;
}
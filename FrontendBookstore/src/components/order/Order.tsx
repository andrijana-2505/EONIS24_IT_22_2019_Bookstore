import { OrderUpdateDto } from '../../dto/order/OrderUpdateDto';

interface OrderProps {
  order: OrderUpdateDto;
}

const Order = ({ order }: OrderProps) => {
  return (
    <div className='order'>
      <h1>{order.ordersId}</h1>

      {/* TODO add order items */}
    </div>
  );
};

export default Order;

import Order from './Order';

interface Address {
  addressId: number;
  street: string;
  city: string;
  postalCode: string;
  ordersId?: number;
  orders?: Order;
}

export default Address;

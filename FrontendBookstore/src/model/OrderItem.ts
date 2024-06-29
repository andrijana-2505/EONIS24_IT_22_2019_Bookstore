import Book from './Book';
import Order from './Order';

interface Orderitem {
  orderItemId: number;
  quantity?: number;
  ordersId?: number;
  bookId?: number;
  book?: Book;
  orders?: Order;
}

export default Orderitem;


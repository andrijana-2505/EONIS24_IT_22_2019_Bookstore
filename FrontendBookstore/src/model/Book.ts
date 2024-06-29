import { Category } from './Category';
import OrderItem from './Orderitem';
import Review from './Review';

interface Book {
  bookId: number;
  bookTitle: string;
  bookAuthor: string;
  publishingYear: string;
  publisher: string;
  bookPrice: number;
  available: number;
  categoryId: null | number;
  category: Category | null;
  orderitems: OrderItem[];
  reviews: Review[];
}

export default Book;

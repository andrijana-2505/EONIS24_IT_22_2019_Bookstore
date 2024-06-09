import OrderItem from './OrderItem';
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
  orderitems: OrderItem[];
  reviews: Review[];
}

export default Book;

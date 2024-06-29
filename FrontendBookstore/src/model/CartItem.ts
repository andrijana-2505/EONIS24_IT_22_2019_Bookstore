import Book from './Book';

export interface CartItem {
  id: number;
  book: Book;
  quantity: number;
}

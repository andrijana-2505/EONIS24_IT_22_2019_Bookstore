import Book from '../../model/Book';

interface BookResponseDto {
  books: Book[];
  pages: number;
  currentPage: number;
}

export default BookResponseDto;

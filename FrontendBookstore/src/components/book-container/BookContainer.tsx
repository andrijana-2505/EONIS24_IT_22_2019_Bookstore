import Book from '../../model/Book';
import './BookContainer.css';
interface BookContainerProps {
  book: Book;
  onClick?: () => void;
}

const BookContainer = ({ book, onClick }: BookContainerProps) => {
  return (
    <div className='book-container' onClick={onClick}>
      <div className='book-container-data'>
        <h2>{book.bookTitle}</h2>
        <p>{book.bookAuthor}</p>
        <p>RSD {book.bookPrice}</p>
        <p>Click to see details</p>
      </div>
    </div>
  );
};

export default BookContainer;

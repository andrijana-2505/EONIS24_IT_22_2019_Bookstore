import { IconCircleArrowLeft, IconCircleArrowRight } from '@tabler/icons-react';
import useBooks from '../../hooks/useBooks';
import Book from '../../model/Book';
import './Books.css';
import BookContainer from '../../components/book-container/BookContainer';
import { useNavigate } from 'react-router-dom';

const Books = () => {
  const { loading, error, data, currentPage, maxPages, setCurrentPage } =
    useBooks();

  const navigate = useNavigate();
  return (
    <div className='books-container'>
      {error && <div className='error-message'>{error}</div>}
      {loading ? (
        <div className='loading-message'>Loading....</div>
      ) : (
        <>
          {data && data.books.length > 0 ? (
            <div className='books-container__books'>
              {data.books.map((book: Book) => (
                <BookContainer
                  key={book.bookId}
                  book={book}
                  onClick={() => navigate(`/book/${book.bookId}`)}
                />
              ))}
            </div>
          ) : (
            <div className='books-container__books empty'>
              <h1>No books in database</h1>
            </div>
          )}
          <div className='books-container__pagination'>
            <button
              onClick={() => setCurrentPage((prevPage) => prevPage - 1)}
              disabled={currentPage === 1 || maxPages === 0}
            >
              <IconCircleArrowLeft size={32} />
            </button>
            <div className='books-container__pagination__pages'>
              {maxPages > 0 ? (
                <>
                  Page <span>{currentPage}</span> of {maxPages}
                </>
              ) : (
                <span>No pages</span>
              )}
            </div>
            <button
              onClick={() => setCurrentPage((prevPage) => prevPage + 1)}
              disabled={currentPage === maxPages || maxPages === 0}
            >
              <IconCircleArrowRight size={32} />
            </button>
          </div>
        </>
      )}
    </div>
  );
};

export default Books;

import { useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import axios from 'axios';
import Book from '../../model/Book';
import './Books.css';

const BooksSearch = () => {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const [categoryId, setCategoryId] = useState<string | null>(null);
  const location = useLocation();

  const fetchBooks = async () => {
    setError('');
    setLoading(true);
    try {
      if (!searchQuery && !categoryId) {
        setBooks([]); // Clear search results if both query and category are empty
        setLoading(false);
        return;
      }

      const response = await axios.get(`http://localhost:5137/api/Book/search`, {
        params: { categoryId, query: searchQuery }
      });

      setBooks(response.data);
      setLoading(false);
    } catch (error) {
      setError(error.response?.data?.message || 'Failed to fetch books');
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBooks();
  }, [location.search, searchQuery, categoryId]);

  const handleSearch = () => {
    fetchBooks();
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(event.target.value);
    if (!event.target.value) {
      setBooks([]); // Clear search results if the search field is empty
    }
  };

  const handleCategoryChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setCategoryId(event.target.value);
    if (!event.target.value) {
      setBooks([]); // Clear search results if the category is not selected
    }
  };

  return (
    <div className='books-page'>
      <div className='search-bar'>
        <input 
          type='text' 
          placeholder='Search by title or author' 
          value={searchQuery} 
          onChange={handleInputChange} 
        /> 
      </div>
      {error && <div className='error-message'>{error}</div>}
      {loading ? (
        <div className='loading-message'>Loading....</div>
      ) : (
        <div className='books-list'>
          {books.map((book) => (
            <div key={book.bookId} className='book-item'>
              <h3>{book.bookTitle}</h3>
              <p>Author: {book.bookAuthor}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default BooksSearch;

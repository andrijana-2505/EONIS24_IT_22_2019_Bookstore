import { useContext } from 'react';
import { BooksContext } from '../context/BookContext';

const useBooks = () => {
  const context = useContext(BooksContext);
  if (context === undefined) {
    throw new Error('useBooks must be used within a BooksContextProvider');
  }
  return context;
};

export default useBooks;

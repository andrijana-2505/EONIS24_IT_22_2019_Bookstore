import React, {useState} from 'react';
import useBooks from '../../hooks/useBooks';
import useCategories from '../../hooks/useCategories';
import {BookCreateDto} from '../../dto/book/BookCreateDto';
import axios from 'axios';
import useAuth from '../../hooks/useAuth';
import {BookUpdateDto} from '../../dto/book/BookUpdateDto';

const BookOperations = () => {
  const [newBook, setNewBook] = useState<BookCreateDto>({
    bookTitle: '',
    bookAuthor: '',
    publishingYear: new Date(),
    publisher: '',
    bookPrice: 0,
    available: 0,
    categoryId: 0,
  });

  const [updatedBook, setUpdatedBook] = useState<BookUpdateDto>({
    bookId: 0,
    bookTitle: '',
    bookAuthor: '',
    publishingYear: new Date(),
    publisher: '',
    bookPrice: 0,
    available: 0,
    categoryId: 0,
  });

  const [selectedBookId, setSelectedBookId] = useState<number>(0);

  const { userToken } = useAuth();
  const { data } = useBooks();
  const { categories } = useCategories();

  const handleCreateInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    // Convert publishingYear to a Date object if it's publishingYear
    // Convert bookPrice to a number if it's bookPrice
    const updatedValue =
      name === 'publishingYear'
        ? new Date(value)
        : name === 'bookPrice'
        ? parseInt(value)
        : value;
    setNewBook({
      ...newBook,
      [name]: updatedValue,
    });
  };

  const handleUpdateInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    // Convert publishingYear to a Date object if it's publishingYear
    // Convert bookPrice to a number if it's bookPrice
    const updatedValue =
      name === 'publishingYear'
        ? new Date(value)
        : name === 'bookPrice'
        ? parseInt(value)
        : value;
    setUpdatedBook({
      ...updatedBook,
      [name]: updatedValue,
    });
  };

  const handleSelectedBookChange = (
    e: React.ChangeEvent<HTMLSelectElement>
  ) => {
    const selectedBookId = Number(e.target.value);
    setSelectedBookId(selectedBookId);
  };

  const handleCreateSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formattedBook = {
      ...newBook,
      publishingYear: newBook.publishingYear.toISOString().split('T')[0],
    };
    try {
      await axios.post(
        `${import.meta.env.VITE_BASE_URL}/api/Book`,
        formattedBook,
        {
          headers: {
            Authorization: `Bearer ${userToken?.token}`,
          },
        }
      );
      alert('Book created');
    } catch (error) {
      console.error(error);
    }
  };

  const handleUpdateSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const bookUpdateDto: BookUpdateDto = {
      bookId: selectedBookId,
      bookTitle: updatedBook.bookTitle,
      bookAuthor: updatedBook.bookAuthor,
      publishingYear: updatedBook.publishingYear,
      publisher: updatedBook.publisher,
      bookPrice: updatedBook.bookPrice,
      available: updatedBook.available,
      categoryId: updatedBook.categoryId,
    };

    const formattedBook = {
      ...bookUpdateDto,
      publishingYear: newBook.publishingYear.toISOString().split('T')[0],
    };
    try {
      await axios.put(
        `${import.meta.env.VITE_BASE_URL}/api/Book`,
        formattedBook,
        {
          headers: {
            Authorization: `Bearer ${userToken?.token}`,
          },
        }
      );
      alert('Book Updated');
    } catch (error) {
      console.error(error);
    }
  };

  const handleDeleteSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    try {
      await axios.delete(
        `${import.meta.env.VITE_BASE_URL}/api/Book/${selectedBookId}`,
        {
          headers: {
            Authorization: `Bearer ${userToken?.token}`,
          },
        }
      );
      alert('Book Deleted');
    } catch (error) {
      console.error(error);
    }
  };


  return (
    <div className='admin-operations'>
      <h1>Book operations</h1>
      <div className='admin-operations__form-container'>
        <h3>Create new book</h3>
        <form
          className='admin-operations__form-container__form'
          onSubmit={handleCreateSubmit}
        >
          <input
            required={true}
            type='text'
            name='bookTitle'
            placeholder='Enter book title ...'
            onChange={handleCreateInputChange}
          />
          <input
            required={true}
            type='text'
            name='bookAuthor'
            placeholder='Enter book author ...'
            onChange={handleCreateInputChange}
          />
          <input
            required={true}
            type='date'
            name='publishingYear'
            placeholder='Enter publishing year ...'
            onChange={handleCreateInputChange}
          />
          <input
            required={true}
            type='text'
            name='publisher'
            placeholder='Enter publisher ...'
            onChange={handleCreateInputChange}
          />
          <input
            required={true}
            type='number'
            name='bookPrice'
            placeholder='Enter book price ...'
            onChange={handleCreateInputChange}
          />
          <input
            required={true}
            type='number'
            name='available'
            placeholder='Enter available copies ...'
            onChange={handleCreateInputChange}
          />
          <select
            id='categoryId'
            name='categoryId'
            onChange={handleCreateInputChange}
          >
            <option>Select...</option>
            {categories && categories.length > 0 ? (
              categories.map((category) => (
                <option key={category.categoryId} value={category.categoryId}>
                  {category.categoryName}
                </option>
              ))
            ) : (
              <option value='' disabled>
                No categories available
              </option>
            )}
          </select>
          <button>Create Book</button>
        </form>
      </div>
      <div className='admin-operations__form-container'>
        <h3>Update book</h3>

        <form
          className='admin-operations__form-container__form'
          onSubmit={handleUpdateSubmit}
        >
          <select id='bookId' name='bookId' onChange={handleSelectedBookChange}>
            <option>Select...</option>
            {data?.books && data?.books.length > 0 ? (
              data?.books.map((book) => (
                <option key={book.bookId} value={book.bookId}>
                  {book.bookTitle}
                </option>
              ))
            ) : (
              <option value='' disabled>
                No books available
              </option>
            )}
          </select>
          <input
            required={true}
            type='text'
            name='bookTitle'
            placeholder='Enter new title ...'
            onChange={handleUpdateInputChange}
          />
          <input
            required={true}
            type='text'
            name='bookAuthor'
            placeholder='Enter new author ...'
            onChange={handleUpdateInputChange}
          />
          <input
            required={true}
            type='date'
            name='publishingYear'
            placeholder='Input new publishing year ...'
            onChange={handleUpdateInputChange}
          />
          <input
            required={true}
            type='text'
            name='publisher'
            placeholder='Enter new publisher ...'
            onChange={handleUpdateInputChange}
          />
          <input
            required={true}
            type='number'
            name='bookPrice'
            placeholder='Enter new book price ...'
            onChange={handleUpdateInputChange}
          />
          <input
            required={true}
            type='number'
            name='available'
            placeholder='Enter new available copies ...'
            onChange={handleUpdateInputChange}
          />
          <select
            id='categoryId'
            name='categoryId'
            onChange={handleUpdateInputChange}
          >
            <option value=''>Select...</option>
            {categories && categories.length > 0 ? (
              categories.map((category) => (
                <option key={category.categoryId} value={category.categoryId}>
                  {category.categoryName}
                </option>
              ))
            ) : (
              <option value='' disabled>
                No categories available
              </option>
            )}
          </select>
          <button disabled={!data?.books || data?.books.length === 0}>
            Update book
          </button>
        </form>
      </div>
      <div className='admin-operations__form-container'>
        <h3>Delete book</h3>

        <form
          className='admin-operations__form-container__form'
          onSubmit={handleDeleteSubmit}
        >
          <select
            id='selectedBookId'
            name='selectedBookId'
            onChange={handleSelectedBookChange}
          >
            <option>Select a book...</option>
            {data?.books && data?.books.length > 0 ? (
              data?.books.map((book) => (
                <option key={book.bookId} value={book.bookId}>
                  {book.bookTitle}
                </option>
              ))
            ) : (
              <option value='' disabled>
                No books available
              </option>
            )}
          </select>
          <button disabled={!data?.books || data?.books.length === 0}>
            Delete book
          </button>
        </form>
      </div>
    </div>
  );
};

export default BookOperations;

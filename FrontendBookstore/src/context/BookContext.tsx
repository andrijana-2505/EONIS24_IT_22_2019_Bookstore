import { createContext, useEffect, useState } from 'react';
import axios from 'axios';
import BookResponseDto from '../dto/book/BookResponseDto';

interface BooksContextProps {
  data: BookResponseDto | null;
  loading: boolean;
  error: string;
  maxPages: number;
  currentPage: number;
  setCurrentPage: React.Dispatch<React.SetStateAction<number>>;
  fetchData: () => void;
}

export const BooksContext = createContext<BooksContextProps | undefined>(
  undefined
);

export const BooksProvider = ({ children }: React.PropsWithChildren) => {
  const [data, setData] = useState<BookResponseDto | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [maxPages, setMaxPages] = useState<number>(1);
  const [currentPage, setCurrentPage] = useState<number>(1);

  const baseUrl = import.meta.env.VITE_BASE_URL;

  const fetchData = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${baseUrl}/api/Book/${currentPage}`);
      if (response.status === 200) {
        const data: BookResponseDto = await response.data;
        // Set data, currentPage and maxPages
        setData(data);
        setCurrentPage(data.currentPage);
        setMaxPages(data.pages);
        setError('');
      } else {
        setError(`Error: ${response.status} ${response.statusText}`);
        console.error(response);
      }
    } catch (error) {
      setError(`${error}`);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [currentPage]);

  return (
    <BooksContext.Provider
      value={{
        data,
        loading,
        error,
        currentPage,
        maxPages,
        setCurrentPage,
        fetchData,
      }}
    >
      {children}
    </BooksContext.Provider>
  );
};

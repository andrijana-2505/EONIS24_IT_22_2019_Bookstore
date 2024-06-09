import { createContext, useEffect, useState } from 'react';
import { Category } from '../model/Category';
import axios from 'axios';

interface CategoryContextProps {
  categories: Category[] | null;
  fetchCategories: () => void;
}

export const CategoryContext = createContext<CategoryContextProps | undefined>(
  undefined
);

export const CategoriesProvider = ({ children }: React.PropsWithChildren) => {
  const [categories, setCategories] = useState<Category[] | null>(null);

  const fetchCategories = async () => {
    try {
      const response = await axios.get(
        `${import.meta.env.VITE_BASE_URL}/api/Category`
      );
      setCategories(response.data);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  return (
    <CategoryContext.Provider value={{ categories, fetchCategories }}>
      {children}
    </CategoryContext.Provider>
  );
};

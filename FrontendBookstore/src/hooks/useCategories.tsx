import { useContext } from 'react';
import { CategoryContext } from '../context/CategoryContext';

const useCategories = () => {
  const context = useContext(CategoryContext);
  if (context === undefined) {
    throw new Error(
      'useCategories must be used within a CategoryContextProvider'
    );
  }
  return context;
};

export default useCategories;

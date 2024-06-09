import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import { AuthProvider } from './context/AuthContext.tsx';
import './index.css';
import { BooksProvider } from './context/BookContext.tsx';
import { CategoriesProvider } from './context/CategoryContext.tsx';
import { CartProvider } from './context/CartContext.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AuthProvider>
      <CategoriesProvider>
        <BooksProvider>
          <CartProvider>
            <App />
          </CartProvider>
        </BooksProvider>
      </CategoriesProvider>
    </AuthProvider>
  </React.StrictMode>
);

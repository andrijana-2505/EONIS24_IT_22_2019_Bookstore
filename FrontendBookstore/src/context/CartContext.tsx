import { createContext, useState } from 'react';
import { CartItem } from '../model/CartItem';

interface CartContextProps {
  cartItems: CartItem[];
  addCartItem: (item: CartItem) => void;
  removeCartItem: (id: number) => void;
}

export const CartContext = createContext<CartContextProps | undefined>(
  undefined
);

export const CartProvider = ({ children }: React.PropsWithChildren) => {
  const [cartItems, setCartItems] = useState<CartItem[]>([]);

  const addCartItem = (item: CartItem) => {
    setCartItems((prevItems) => [...prevItems, item]);
  };

  const removeCartItem = (id: number) => {
    setCartItems((prevItems) =>
      prevItems.filter((item) => item.book.bookId === id)
    );
  };

  return (
    <CartContext.Provider value={{ cartItems, addCartItem, removeCartItem }}>
      {children}
    </CartContext.Provider>
  );
};

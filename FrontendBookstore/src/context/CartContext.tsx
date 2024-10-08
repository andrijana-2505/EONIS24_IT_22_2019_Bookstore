import { createContext, useState, useEffect } from 'react';
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
  useEffect(() => {
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
  }, [cartItems]);

  const removeCartItem = (id: number) => {
    setCartItems((prevItems) =>
      prevItems.filter((item) => item.id !== id)
    );
  };

  return (
    <CartContext.Provider value={{ cartItems, addCartItem, removeCartItem }}>
      {children}
    </CartContext.Provider>
  );
};

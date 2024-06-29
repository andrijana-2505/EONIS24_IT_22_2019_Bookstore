import Review from './Review';
import UserRole from './UserRole';
import Order from './Order';

interface User {
  usersId: number;
  userRole: UserRole; 
  username: string;
  passwordLogin: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  genre?: string;
  orders: Order[];
  reviews: Review[];
}

export default User;

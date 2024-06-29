import { Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import AdminDashboard from './pages/admin-dashboard/AdminDashboard';
import LoginPage from './pages/auth-pages/LoginPage';
import RegisterPage from './pages/auth-pages/RegisterPage';
import BookDetails from './pages/book-details/BookDetails';
import Books from './pages/books/Books';
import Cart from './pages/cart/Cart';
import Error from './pages/error/Error';
import Landing from './pages/landing/Landing';
import Main from './pages/main/Main';
import Profile from './pages/profile/Profile';
import Unauthorized from './pages/unathorized/Unauthorized';
import AdminRoutes from './utils/AdminRoutes';
import PrivateRoutes from './utils/PrivateRoutes';
import CategoryOperations from './components/category-operations/CategoryOperations';
import BookOperations from './components/book-operations/BookOperations';
import Success from './pages/stripe-pages/success/Success';
import AboutUs from './components/aboutUs/AboutUs';
import UserList from './components/users-list/UserList';
import EditUser from './components/edit-user/EditUser';
import OrderHistory from './components/order-history/OrderHistory';
import UpdateOrder from './components/update-order/UpdateOrder';

function App() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<Main />}>
          <Route index element={<Landing />} />
          <Route path='/books' element={<Books />} />
          <Route path="/about-us" element={<AboutUs />} />
          <Route path='/book/:bookId' element={<BookDetails />} />
          <Route path='/login' element={<LoginPage />} />
          <Route path='/register' element={<RegisterPage />} />
          <Route element={<PrivateRoutes />}>
            <Route path='/profile' element={<Profile />} />
            <Route path='/cart' element={<Cart />} />
          </Route>

          <Route path='/unauthorized' element={<Unauthorized />} />
          <Route path='*' element={<Error />} />
        </Route>
        <Route element={<AdminRoutes />}>
          <Route path='/editUser/:userId' element={<EditUser />} />
          <Route path='/admin-dashboard/*' element={<AdminDashboard />}>
            <Route index element={<CategoryOperations />} />
            <Route path='book-operations' element={<BookOperations />} />
            <Route path='order-history' element={<OrderHistory />} />
              <Route path='update-order/:orderId' element={<UpdateOrder />} />
            <Route path='users-list' element={<UserList />} />
          </Route>
        </Route>
        <Route path='/success' element={<Success />} />
      </Routes>
    </Router>
  );
}

export default App;

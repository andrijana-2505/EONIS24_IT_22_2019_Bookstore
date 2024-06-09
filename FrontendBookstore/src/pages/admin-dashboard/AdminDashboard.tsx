import { Link, Outlet } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import './AdminDashboard.css';

const AdminDashboard = () => {
  const { userData, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <div className='admin-dashboard'>
      <div className='admin-dashboard__sidebar'>
        <h1>Welcome back Admin: {userData?.username}</h1>
        <Link to='/' onClick={handleLogout}>
          Logout
        </Link>
        <Link to='/'>Back to home page</Link>
        <Link to='/admin-dashboard'>Category Operations</Link>
        <Link to='/admin-dashboard/book-operations'>Book Operations</Link>
      </div>
      <div className='admin-dashboard__content'>
        <Outlet />
      </div>
    </div>
  );
};

export default AdminDashboard;

import { Link, useNavigate } from 'react-router-dom';
import './Navbar.css';
import useAuth from '../../hooks/useAuth';

const Navbar = () => {
  const navigate = useNavigate();
  const { userData, isAuthenticated, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <div className='navbar'>
      <Link to='/' className='navbar__site-logo'>
        BOOKSTORE
      </Link>
      <div className='navbar_nav-links'>
        <Link to='/' className='navbar_nav-links__nav-link'>
          Home
        </Link>
        <Link to='/books' className='navbar_nav-links__nav-link'>
          Books
        </Link>
        <Link to='/about-us' className='navbar_nav-links__nav-link'>
          About us
        </Link>

        {isAuthenticated ? (
          <>
            {Number(userData?.userRole) === 1 && (
              <Link
                to='/admin-dashboard'
                className='navbar_nav-links__nav-link'
              >
                Admin dashboard
              </Link>
            )}
            <Link to='/cart' className='navbar_nav-links__nav-link'>
              Cart
            </Link>
            <Link to='/profile' className='navbar_nav-links__nav-link'>
              Profile
            </Link>
            <div className='navbar_nav-links__auth-btn' onClick={handleLogout}>
              Logout
            </div>
          </>
        ) : (
          <Link to='/register' className='navbar_nav-links__auth-btn'>
            Sign up
          </Link>
        )}
      </div>
    </div>
  );
};

export default Navbar;

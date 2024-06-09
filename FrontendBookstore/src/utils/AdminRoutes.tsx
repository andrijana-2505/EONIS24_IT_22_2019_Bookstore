import { Navigate, Outlet } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

const AdminRoutes = () => {
  const { userData, loading } = useAuth();

  if (loading) {
    return null;
  }

  return Number(userData?.userRole) === 1 ? (
    <Outlet />
  ) : (
    <Navigate to='/unauthorized' />
  );
};

export default AdminRoutes;

import { Outlet } from 'react-router-dom';
import Navbar from '../../components/navbar/Navbar';
import './Main.css';
import Footer from '../../components/footer/Footer';

const Main = () => {
  return (
    <div className='main'>
      <Navbar />
      <div className='content'>
        <Outlet />
      </div>
      <Footer />
    </div>
  );
};

export default Main;

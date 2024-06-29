import { useNavigate } from 'react-router-dom';
import './Landing.css';
import BooksSearch from '../books/BooksSearch';

const Landing = () => {
  const navigate = useNavigate();

  return (
    <div className='landing-page'>
      <div className='landing-page__left-section'>
        <span className='landing-page__left-section__section-text large'>
          With us you can read over 7000 free books!
        </span>
        <span className='landing-page__left-section__section-text small'>
          We offer access to a growing library of over 7000 free books
        </span>
        <div className='landing-page-search'>
          <BooksSearch />
        </div>
        <span className='landing-page__left-section__section-text small'>
          Join our community and start reading today!{' '}
          <span onClick={() => navigate('/register')}>Create an account</span>
        </span>
      </div>
      <div className='landing-page__right-section'>
        <img src='/landing-page-img.png' style={{ width: '700px' }} />
      </div>
    </div>
  );
};

export default Landing;

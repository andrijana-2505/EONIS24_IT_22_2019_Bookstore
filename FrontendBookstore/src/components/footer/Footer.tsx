import './Footer.css';

const Footer = () => {
  const year = new Date().getFullYear();

  return (
    <div className='footer'>
      <p>© {year} BOOKSTORE All rights reserved.</p>
      <a href='#'>Privacy Policy</a>
      <a href='#'>Terms of Service</a>
    </div>
  );
};

export default Footer;

import React from 'react';
import './AboutUs.css';


const AboutUs = () => {
  return (
    <div className="about-us" style={{ backgroundImage: `url(${import.meta.env.BASE_URL}stacked-books.jpeg)` }}>
      <div className="about-us__content">
        <h2>About Our Bookstore</h2>
        <p>
        Welcome to our bookstore! At our bookstore, we're passionate about bringing you the best in 
        literature, from timeless classics to thrilling adventures and thought-provoking stories. 
        Whether you're a fiction enthusiast, a science fiction fanatic, or someone looking for a gripping crime novel, 
        we've got something for everyone.
        </p>
        <p>
        Our mission is to create a space where book lovers can explore, discover, 
        and indulge in their literary passions. From the latest bestsellers to hidden gems waiting to be unearthed, 
        our shelves are stocked to inspire and delight.
        </p>
        <p>
        Join us on this journey through the pages of great books and 
        let us help you find your next favorite read. 
        Happy reading!
        </p>
      </div>
    </div>
  );
};

export default AboutUs;

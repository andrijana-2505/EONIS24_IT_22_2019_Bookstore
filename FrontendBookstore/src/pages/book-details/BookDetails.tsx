import axios from 'axios';
import React, {useEffect, useState} from 'react';
import {useNavigate, useParams} from 'react-router-dom';
import useAuth from '../../hooks/useAuth';
import useCart from '../../hooks/useCart';
import Book from '../../model/Book';
import {CartItem} from '../../model/CartItem';
import './BookDetails.css';
import Review from '../../model/Review';
import BookReview from "../../components/book-review/BookReview.tsx";
import { v4 as uuidv4 } from 'uuid';


// TODO backend issue -> not getting reviews
const BookDetails = () => {
    const {bookId} = useParams();
    const [bookData, setBookData] = useState<Book | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [reviewRating, setReviewRating] = useState(1);

    // Quantity
    const [quantity, setQuantity] = useState<number>(1);

    // Hooks
    const {addCartItem} = useCart();
    const {isAuthenticated, userData, userToken} = useAuth();
    const navigate = useNavigate();

    const baseUrl = import.meta.env.VITE_BASE_URL;

    const fetchBookDetails = async () => {
        setError(''); // Clear previous error
        setLoading(true);
        try {
            // ! error on backend side
            // ! not returning reviews on this specific request
            const response = await axios.get(`${baseUrl}/api/Book/ids/${bookId}`);
            setBookData(response.data[0]);
            console.log(response.data);
            setLoading(false);
        } catch (error) {
            setError(error.response?.data?.message || 'Failed to fetch book details');
            setLoading(false);
        }
    };

    const handleAddToCart = () => {
        if (!isAuthenticated) {

            navigate('/login')
            return;
        }
        if (bookData !== null && quantity > 0) {
            const cartItem: CartItem = {

                id: uuidv4(),
                book: bookData,
                quantity: quantity,
            };

            alert('Item added to cart');
            addCartItem(cartItem);
        }
    };

    const handleReview = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!isAuthenticated) {
            navigate('/login');
            return;
        }

        setError(''); // Clear previous error
        setLoading(true);
        try {
            const review: Review = {
                reviewDate: new Date().toISOString().split('T')[0], // Change this line
                usersId: Number(userData?.usersId),
                bookId: Number(bookId),
                rating: reviewRating,
            };

            console.log(JSON.stringify(review));

            await axios.post(`${baseUrl}/api/Review`, review, {
                headers: {
                    Authorization: `Bearer ${userToken?.token}`,
                },
            });

            fetchBookDetails();
            setLoading(false);
            alert('UserReview added');
        } catch (error) {
            setError(error.response?.data?.message || 'Failed to submit review');
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchBookDetails();
    }, []);

    const handleQuantityChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setQuantity(Number(event.target.value));
    };

    const handleRatingChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setReviewRating(Number(e.target.value));
    };

    return (
        <div className='book-details'>
            {error && <div className='error-message'>{error}</div>}
            {loading ? (
                <div className='loading-message'>Loading....</div>
            ) : (
                <div className='book-details__book-info'>
                    <h2>{bookData?.bookTitle}</h2>
                    <h3>Author: {bookData?.bookAuthor}</h3>
                    <p>
                        Publisher: {bookData?.publisher} - {bookData?.publishingYear}
                    </p>
                    <p>Available copies: {bookData?.available}</p>

                    <label htmlFor='quantity'>Quantity:</label>
                    <input
                        type='number'
                        id='quantity'
                        name='quantity'
                        value={quantity}
                        min={1}
                        onChange={handleQuantityChange}
                    />
                    <button
                        className='book-details__book-info__cart-btn'
                        onClick={handleAddToCart}
                    >
                        ADD TO CART
                    </button>
                    <form
                        className='book-details__book-info__review-form'
                        onSubmit={handleReview}
                    >
                        <div className='book-details__book-info__review-form___review-rating__dropdown'>
                            <label htmlFor='reviewRating'>Add your rating</label>
                            <select
                                id='reviewRating'
                                name='reviewRating'
                                className='book-details__book-info__review-form__review-rating__dropdown'
                                onChange={handleRatingChange}
                                value={reviewRating}
                            >
                                <option value='1'>1</option>
                                <option value='2'>2</option>
                                <option value='3'>3</option>
                                <option value='4'>4</option>
                                <option value='5'>5</option>
                            </select>
                        </div>

                        <button type='submit'>Submit review</button>
                    </form>
                    <div className='book-details-reviews'>{bookData?.reviews && bookData.reviews.length > 0 ? (
                        bookData.reviews.map((review) => (
                            <BookReview key={review.reviewId} review={review}/>
                        ))
                    ) : (
                        <p>No reviews</p>
                    )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default BookDetails;

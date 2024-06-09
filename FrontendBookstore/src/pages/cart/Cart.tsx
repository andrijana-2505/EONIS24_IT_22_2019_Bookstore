import {loadStripe} from '@stripe/stripe-js';
import axios from 'axios';
import {StripeRequestDto} from '../../dto/StripeRequestDto';
import useCart from '../../hooks/useCart';
import './Cart.css';
import React, {useState} from "react";

const Cart = () => {
    const {cartItems} = useCart();
    console.log(cartItems);

    const [street, setStreet] = useState('')
    const [postalCode, setPostalCode] = useState('')
    const [city, setCity] = useState('')

    const stripe_public_key =
        'pk_test_51PO3zKRshBAKvgM053XkLxJwKMG620oywnDThYnRdbnzKwSSnIwU88OdUmKIivQTnDCwaKb730ZSRwGrfk9Ht7l200rch4aHdz';
    const stripePromise = loadStripe(stripe_public_key);

    const handleCheckout = async () => {
        const stripeRequestDto: StripeRequestDto = {
            orderItems: cartItems.map((item) => ({
                name: item.book.bookTitle,
                unitAmount: parseInt(String(item.book.bookPrice), 10), // Convert to cents
                quantity: item.quantity,
            })),
            orderId: 1,
            street: street,
            postalCode: postalCode,
            city: city,
        };

        try {
            const response = await axios.post(
                'http://localhost:5137/api/create-checkout-session',
                stripeRequestDto,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );

            if (response.status !== 200) {
                console.error('Failed to create checkout session', response);
                return;
            }

            const session = response.data;

            if (!session.id) {
                console.error('Session ID is missing in the response', session);
                return;
            }

            const stripe = await stripePromise;

            const {error} = await stripe!.redirectToCheckout({
                sessionId: session.id,
            });

            if (error) {
                console.error('Error redirecting to checkout:', error);
            }
        } catch (error) {
            console.error('Error creating checkout session:', error);
        }
    };

    // Calculate the total price
    const totalPrice = cartItems.reduce((total, cartItem) => {
        return total + cartItem.book.bookPrice * cartItem.quantity;
    }, 0);

    const handleStreetChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setStreet(e.target.value)
    }

    const handlePostalCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPostalCode(e.target.value)
    }

    const handleCityChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setCity(e.target.value)
    }

    return (
        <div className='cart'>
            {cartItems && cartItems.length > 0 ? (
                cartItems.map((cartItem, index) => (
                    <>

                        <div key={index} className='cart__cart-item'>
                            <div>
                                <h3>{cartItem.book.bookTitle}</h3>
                                <div>Price: ${cartItem.book.bookPrice}</div>
                                <div>Quantity: {cartItem.quantity}</div>
                            </div>
                        </div>
                        <div className='cart__address-fields'>
                            <form className='cart__adress-fields-form'>
                                <input type="text" name="" id="" placeholder='Street' onChange={handleStreetChange}/>
                                <input type="text" name="" id="" placeholder='Postal code'
                                       onChange={handlePostalCodeChange}/>
                                <input type="text" name="" id="" placeholder='City' onChange={handleCityChange}/>
                            </form>
                        </div>
                    </>
                ))
            ) : (
                <h1>No items in cart</h1>
            )}

            <div className='cart__total-price'>Total Price: ${totalPrice}</div>
            <button disabled={cartItems.length === 0} onClick={handleCheckout}>
                CONTINUE
            </button>
        </div>
    );
};

export default Cart;

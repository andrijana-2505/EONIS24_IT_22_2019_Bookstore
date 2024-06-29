import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import UserCreateDto from '../../dto/user/UserCreateDto';
import useAuth from '../../hooks/useAuth';
import './Auth.css';

// TODO backend issue gender is not being set
const RegisterPage = () => {
  const [registerFormData, setRegisterFormData] = useState<UserCreateDto>({
    username: '',
    password: '',
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    gender: '',
  });

  const [registerFormErrors, setRegisterFormErrors] = useState<
    Partial<UserCreateDto>
  >({});

  const { isAuthenticated, register, error, clearError } = useAuth();

  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/');
    }
    return () => {
      clearError();
    };
  }, [isAuthenticated]);

  const validateRegisterForm = (formData: UserCreateDto) => {
    const newErrors: Partial<UserCreateDto> = {};
    if (!formData.username.trim()) {
      newErrors.username = 'Username is required';
    }
    if (!formData.password.trim()) {
      newErrors.password = 'Password is required';
    }
    if (!formData.firstName.trim()) {
      newErrors.firstName = 'First name is required';
    }
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Last name is required';
    }
    if (!formData.email.trim()) {
      newErrors.email = 'Email is required';
    }
    if (!formData.phone?.trim()) {
      newErrors.phone = 'Phone number is required';
    }
    if (!formData.gender?.trim()) {
      newErrors.gender = 'Gender is required';
    }
    setRegisterFormErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleRegister = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();


    if (validateRegisterForm(registerFormData)) {
      const success = await register(registerFormData);
      if (success) {
        console.log('success');
        alert('User registered successfully');
      } else {
        console.log(error);
      }
    }

    setRegisterFormData({
      username: '',
      password: '',
      firstName: '',
      lastName: '',
      email: '',
      phone: '',
      gender: '',
    });
  };

  const handleRegisterInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setRegisterFormData({
      ...registerFormData,
      [name]: value,
    });
  };

  return (
    <div className='auth-container'>
      <div className='auth-container-section'>
        <h2>Create an account</h2>
        <p>Fill out the form to create a new account for this app</p>
      </div>
      <form onSubmit={handleRegister} className='auth-container-section inputs'>
        <input
          type='text'
          name='username'
          placeholder='Username'
          value={registerFormData.username}
          onChange={handleRegisterInputChange}
        />
        {registerFormErrors.username && (
          <div className='error-message'>{registerFormErrors.username}</div>
        )}
        <input
          type='text'
          name='firstName'
          placeholder='First name'
          value={registerFormData.firstName}
          onChange={handleRegisterInputChange}
        />
        {registerFormErrors.firstName && (
          <div className='error-message'>{registerFormErrors.firstName}</div>
        )}
        <input
          type='text'
          name='lastName'
          placeholder='Last name'
          value={registerFormData.lastName}
          onChange={handleRegisterInputChange}
        />
        {registerFormErrors.lastName && (
          <div className='error-message'>{registerFormErrors.lastName}</div>
        )}
        <input
          type='email'
          name='email'
          placeholder='email@domain.com'
          value={registerFormData.email}
          onChange={handleRegisterInputChange}
        />
        {registerFormErrors.email && (
          <div className='error-message'>{registerFormErrors.email}</div>
        )}
        <input
          type='password'
          name='password'
          placeholder='Password'
          value={registerFormData.password}
          onChange={handleRegisterInputChange}
        />
        {registerFormErrors.password && (
          <div className='error-message'>{registerFormErrors.password}</div>
        )}
        <input
          type='text'
          name='phone'
          placeholder='Phone'
          value={registerFormData.phone}
          onChange={handleRegisterInputChange}
        />
        {registerFormErrors.phone && (
          <div className='error-message'>{registerFormErrors.phone}</div>
        )}
        <div className='gender-selection'>
          <label htmlFor='gender'>Select your gender:</label>
          <select
            id='gender'
            name='gender'
            value={registerFormData.gender}
            onChange={handleRegisterInputChange}
          >
            <option value=''>Select...</option>
            <option value='male'>Male</option>
            <option value='female'>Female</option>
          </select>
        </div>
        {registerFormErrors.gender && (
          <div className='error-message'>{registerFormErrors.gender}</div>
        )}
        <button type='submit' className='auth-button'>
          Register
        </button>
      </form>
      <div className='auth-container-section'>
        <div className='auth-redirect'>
          Click <span onClick={() => navigate('/login')}>here</span> to login
        </div>
      </div>
      <div className='auth-container-section'>
        <p className='terms-of-service-text'>
          By creating an account, you agree to our <span>Terms of Service</span>{' '}
          and <span>Privacy Policy</span>
        </p>
      </div>
      {error && <div className='error-message'>{error}</div>}
    </div>
  );
};

export default RegisterPage;

import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import UserLoginDto from '../../dto/user/UserLoginDto';
import useAuth from '../../hooks/useAuth';
import './Auth.css';

const LoginPage = () => {
  const [loginFormData, setLoginFormData] = useState<UserLoginDto>({
    email: '',
    password: '',
  });

  const [loginFormErrors, setLoginFormErrors] = useState<Partial<UserLoginDto>>(
    {}
  );

  const { isAuthenticated, login, error, clearError } = useAuth();

  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/');
    }
    return () => {
      clearError();
    };
  }, [isAuthenticated]);

  const handleLoginInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setLoginFormData({
      ...loginFormData,
      [name]: value,
    });
  };

  const validateLoginForm = (formData: UserLoginDto) => {
    const newErrors: Partial<UserLoginDto> = {};
    if (!formData.email.trim()) {
      newErrors.email = 'Username is required';
    }
    if (!formData.password.trim()) {
      newErrors.password = 'Password is required';
    }
    setLoginFormErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleLogin = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (validateLoginForm(loginFormData)) {
      const success = await login(loginFormData.email, loginFormData.password);
      if (success) {
        console.log('success');

        navigate('/');
        alert('User logged in successfully');
      } else {
        console.log(error);
      }
    }

    setLoginFormData({
      email: '',
      password: '',
    });
  };

  return (
    <div className='auth-container'>
      <div className='auth-container-section'>
        <h2>Login</h2>
        <p>Enter your email and password to login</p>
      </div>
      <form onSubmit={handleLogin} className='auth-container-section inputs'>
        <input
          type='text'
          name='email'
          placeholder='Email'
          value={loginFormData.email}
          onChange={handleLoginInputChange}
        />
        {loginFormErrors.email && (
          <div className='error-message'>{loginFormErrors.email}</div>
        )}
        <input
          type='password'
          name='password'
          placeholder='Password'
          value={loginFormData.password}
          onChange={handleLoginInputChange}
        />
        {loginFormErrors.password && (
          <div className='error-message'>{loginFormErrors.password}</div>
        )}
        <button type='submit' className='auth-button'>
          Login
        </button>
      </form>
      <div className='auth-container-section'>
        <div className='auth-redirect'>
          Click <span onClick={() => navigate('/register')}>here</span> to
          register
        </div>
      </div>
      {error && <div className='error-message'>{error}</div>}
    </div>
  );
};

export default LoginPage;

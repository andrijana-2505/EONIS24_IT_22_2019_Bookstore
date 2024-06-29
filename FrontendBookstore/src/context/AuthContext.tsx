import React, { createContext, useEffect, useState } from 'react';
import AuthUserToken from '../model/AuthUserToken';
import UserCreateDto from '../dto/user/UserCreateDto';
import axios from 'axios';
import { UserReadDto } from '../dto/user/UserReadDto';

interface AuthContextProps {
  userToken: AuthUserToken | null;
  userData: UserReadDto | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string;
  login: (email: string, password: string) => Promise<boolean>;
  register: (formData: UserCreateDto) => Promise<boolean>;
  logout: () => void;
  clearError: () => void;
}

export const AuthContext = createContext<AuthContextProps | undefined>(
  undefined
);

export const AuthProvider = ({ children }: React.PropsWithChildren) => {
  const [userToken, setUserToken] = useState<AuthUserToken | null>(null);
  const [userData, setUserData] = useState<UserReadDto | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const userToken = localStorage.getItem('userToken');
    if (userToken) {
      try {
        const parsedUserToken = JSON.parse(userToken) as AuthUserToken;
        setUserToken(parsedUserToken);
        setIsAuthenticated(true);
      } catch (error) {
        setError('Error parsing user token');
      }
    } else {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    const fetchUserData = async () => {
      if (userToken) {
        setLoading(true);
        try {
          const response = await axios.get(
            'http://localhost:5137/api/User/my-data',
            {
              headers: {
                Authorization: `Bearer ${userToken.token}`,
                'Content-Type': 'application/json',
              },
            }
          );
          setUserData(response.data);
        } catch (error) {
          setError('Error fetching user data');
        } finally {
          setLoading(false);
        }
      }
    };

    fetchUserData();
  }, [userToken]);

  const login = async (email: string, password: string): Promise<boolean> => {
    setLoading(true);
    try {
      const response = await axios.post(
        'http://localhost:5137/api/Auth/login',
        {
          email,
          password,
        },
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );

      if (response.status === 200) {
        const data = response.data;
        const AuthUserToken: AuthUserToken = {
          token: data,
        };
        console.log({
          data: data,
          response: { responseStatus: response.status, response: response },
        });
        localStorage.setItem('userToken', JSON.stringify(AuthUserToken));
        setUserToken(AuthUserToken);
        setIsAuthenticated(true);
        return true;
      }
    } catch (error) {
      if (
        axios.isAxiosError(error) &&
        error.response &&
        error.response.status === 400
      ) {
        setError(
          typeof error.response.data === 'string'
            ? error.response.data
            : error.message
        );
      } else {
        setError((error as Error).message);
      }
      return false;
    } finally {
      setLoading(false);
    }
    return false;
  };

  const register = async (formData: UserCreateDto): Promise<boolean> => {
    setLoading(true);
    try {
      const response = await axios.post(
        'http://localhost:5137/api/Auth/register',
        { ...formData },
        {
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );

      if (response.status === 200) {
        const data = response.data;
        console.log({
          data: data,
          response: { responseStatus: response.status, response: response },
        });
        return true;
      }
    } catch (error) {
      if (
        axios.isAxiosError(error) &&
        error.response &&
        error.response.status === 400
      ) {
        setError(
          typeof error.response.data === 'string'
            ? error.response.data
            : error.message
        );
      } else {
        setError((error as Error).message);
      }
      return false;
    } finally {
      setLoading(false);
    }
    return false;
  };

  const logout = async () => {
    setLoading(true);
    try {
      localStorage.removeItem('userToken');
      setUserToken(null);
      setIsAuthenticated(false);
    } catch (error) {
      setError((error as Error).message);
    } finally {
      setLoading(false);
    }
  };

  const clearError = () => {
    setError('');
  };

  return (
    <AuthContext.Provider
      value={{
        userToken,
        userData,
        error,
        loading,
        isAuthenticated,
        clearError,
        login,
        logout,
        register,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

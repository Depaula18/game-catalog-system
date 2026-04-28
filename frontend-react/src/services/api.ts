import axios from 'axios';

export const api = axios.create({
  //baseURL: 'https://localhost:7277/api',
  baseURL: 'https://game-catalog-api-xk1h.onrender.com'
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('@GameCatalog:token');

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);
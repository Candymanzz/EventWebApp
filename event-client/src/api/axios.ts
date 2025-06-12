import axios from 'axios';

const api = axios.create({
    baseURL: process.env.VITE_API_URL || 'http://localhost:5114/api',
    withCredentials: false,
});

export default api;

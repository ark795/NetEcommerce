import axios from "axios";

const axiosInstance = axios.create({
    baseURL: process.env.NEXT_PUBLIC_AUTH_SERVICE,
    // secondURL: process.env.NEXT_PUBLIC_CATALOG_SERVICE || 'http://localhost:5002',
})

export default axiosInstance
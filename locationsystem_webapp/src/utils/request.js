// 导入axios
import axios from 'axios';
import router from '../router'


// 导入element-ui的Message组件用于消息提醒
// 创建axios实例
const service = axios.create({
    baseURL: "/", // API的基础URL
    
});

let isRefreshing = false;

// 请求拦截器
service.interceptors.request.use(
    config => {
        // 在发送请求之前做一些处理，比如添加token
        
        if (config.url != "/api/Login/Login?type=user") {
            const token = localStorage.getItem('access_token');
            if (token) {
                config.headers['Authorization'] = "Bearer "+token;
            }
        }
        config.headers['Content-Type'] = "application/json";
        return config;
    },
    error => {
        // 处理请求错误
        return Promise.reject(error);
    }
);
// 响应拦截器
service.interceptors.response.use(
    response => {
        // 对响应数据做一些处理
        const res = response.data;
        if (response.status === 200) {
            return response;
        } else if (response.status === 401) {
            return router?.push('/login')
        }
        else {
            return Promise.reject(new Error(res.message || 'Error'));
        }
    },
    async error => {
        // 处理响应错误
        const originalRequest = error.config;
        if (error.response?.status === 401 && !originalRequest._retry)
        {
            if (isRefreshing) {
                return service(originalRequest);
            }
            
            // 清除token
            
        } else {
            return Promise.reject(error);
        }
    }
);
export default service;

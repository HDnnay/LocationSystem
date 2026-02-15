// 导入axios
import axios from 'axios';
import authService from '../utils/authService';
import authStorage from '../utils/authStorage';

// 创建axios实例
const service = axios.create({
    baseURL: "/", // API的基础URL

});

// 请求拦截器
service.interceptors.request.use(
    config => {
        // 在发送请求之前做一些处理，比如添加token
        const token = authStorage.getAccessToken();
        // 当请求的是 /api/auth/refresh-token 接口时，不添加 Authorization 头
        if (token && !config.url.includes('/api/auth/refresh-token')) {
            config.headers['Authorization'] = "Bearer "+token;
        }
        // 只有在不是文件上传时才设置Content-Type为application/json
        if (!config.headers['Content-Type'] || config.headers['Content-Type'] !== 'multipart/form-data') {
            config.headers['Content-Type'] = "application/json";
        }
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
            return response.data;
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
            if (authService.getIsRefreshing()) {
                // 正在刷新token，等待刷新完成后重试
                try {
                    const newToken = await authService.refreshToken();
                    originalRequest.headers.Authorization = `Bearer ${newToken}`;
                    return service(originalRequest);
                } catch (err) {
                    return Promise.reject(err);
                }
            }

            originalRequest._retry = true;
            try {
                const newToken = await authService.refreshToken();
                originalRequest.headers.Authorization = `Bearer ${newToken}`;
                return service(originalRequest);
            } catch (err) {
                return Promise.reject(err);
            }
        } else {
            return Promise.reject(error);
        }
    }
);
export default service;

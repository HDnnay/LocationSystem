// 导入axios
import axios from 'axios';



// 导入element-ui的Message组件用于消息提醒
// 创建axios实例
const service = axios.create({
    baseURL: "/", // API的基础URL

});

let isRefreshing = false;
let refreshSubscribers = [];

// 请求拦截器
service.interceptors.request.use(
    config => {
        // 在发送请求之前做一些处理，比如添加token
        const token = localStorage.getItem('access_token');
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

// 通知所有订阅者
function notifySubscribers(newToken) {
    refreshSubscribers.forEach(callback => callback(newToken));
    refreshSubscribers = [];
}

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
            if (isRefreshing) {
                // 正在刷新token，将请求加入队列
                return new Promise(resolve => {
                    refreshSubscribers.push(newToken => {
                        originalRequest.headers.Authorization = `Bearer ${newToken}`;
                        resolve(service(originalRequest));
                    });
                });
            }
            isRefreshing = true;
            originalRequest._retry = true;
            try {
                const refreshToken = localStorage.getItem('refresh_token');
                const userType = localStorage.getItem('user_type');
                if (refreshToken && userType) {
                    const res = await service.post('/api/auth/refresh-token', {
                        RefreshToken: refreshToken
                    });
                    if (res.accessToken) {
                        let newToken = res.accessToken;
                        let newRefreshToken = res.refreshToken;
                        localStorage.setItem('access_token', newToken);
                        localStorage.setItem('refresh_token', newRefreshToken);
                        // 更新用户信息
                        if (res.userInfo) {
                            localStorage.setItem('user_info', JSON.stringify(res.userInfo));
                        }
                        originalRequest.headers.Authorization = `Bearer ${newToken}`;
                        // 通知所有订阅者
                        notifySubscribers(newToken);
                        return service(originalRequest);
                    }
                }
                // 如果refreshToken或userType不存在，清除本地存储并跳转到登录页
                localStorage.removeItem('access_token');
                localStorage.removeItem('refresh_token');
                localStorage.removeItem('user_type');
                localStorage.removeItem('user_info');
                // 跳转到登录页
                window.location.href = '/login';
                return Promise.reject(new Error('登录已过期，请重新登录'));
            } catch (err) {
                localStorage.removeItem('access_token');
                localStorage.removeItem('refresh_token');
                localStorage.removeItem('user_type');
                localStorage.removeItem('user_info');
                // 跳转到登录页
                window.location.href = '/login';
                return Promise.reject(err);
            } finally {
                isRefreshing = false;
            }
        } else {
            return Promise.reject(error);
        }
    }
);
export default service;

// 导入axios
import axios from 'axios';



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
        const token = localStorage.getItem('access_token');
        if (token) {
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
            return response;
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
            isRefreshing = true;
            originalRequest._retry = true;
            try {
                const refreshToken = localStorage.getItem('refresh_token');
                const userType = localStorage.getItem('user_type');
                if (refreshToken && userType) {
                    await service.post('/api/auth/refresh-token', {
                        RefreshToken: refreshToken,
                        Type: parseInt(userType)
                    }).then(res => {
                        if (res.status === 200) {
                            let newToken = res.data.data.accessToken;
                            let newRefreshToken = res.data.data.refreshToken;
                            localStorage.setItem('access_token', newToken);
                            localStorage.setItem('refresh_token', newRefreshToken);
                            originalRequest.headers.Authorization = `Bearer ${newToken}`;
                            return service(originalRequest);
                        }
                    });
                }
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

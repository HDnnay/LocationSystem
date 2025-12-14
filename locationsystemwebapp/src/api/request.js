import axios from 'axios'

// 创建 axios 实例
const service = axios.create({
  // 基础URL，配合 vite.config.js 中的代理配置
  baseURL: '/api',
  // 请求超时时间
  timeout: 10000,
  // 请求头
  headers: {
    'Content-Type': 'application/json'
  }
})

// 请求拦截器
service.interceptors.request.use(
  config => {
    // 从本地存储获取 token
    const token = localStorage.getItem('token')
    // 如果 token 存在，则在请求头中添加 token
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  error => {
    // 处理请求错误
    console.error('请求错误:', error)
    return Promise.reject(error)
  }
)

// 响应拦截器
service.interceptors.response.use(
  response => {
    // 只返回响应数据
    return response.data
  },
  error => {
    // 处理响应错误
    if (error.response) {
      // HTTP 错误状态码处理
      switch (error.response.status) {
        case 401:
          // 未授权，重定向到登录页
          console.error('未授权，请重新登录')
          // 可以在这里添加重定向到登录页的逻辑
          break
        case 403:
          console.error('拒绝访问')
          break
        case 404:
          console.error('请求的资源不存在')
          break
        case 500:
          console.error('服务器内部错误')
          break
        default:
          console.error(`请求错误，状态码：${error.response.status}`)
      }
    } else if (error.request) {
      // 请求已发送但没有收到响应
      console.error('网络错误，无法连接到服务器')
    } else {
      // 请求配置错误
      console.error('请求配置错误:', error.message)
    }
    return Promise.reject(error)
  }
)

// 封装常用的请求方法
const request = {
  // GET 请求
  get(url, params = {}) {
    return service({
      url,
      method: 'get',
      params
    })
  },

  // POST 请求
  post(url, data = {}) {
    return service({
      url,
      method: 'post',
      data
    })
  },

  // PUT 请求
  put(url, data = {}) {
    return service({
      url,
      method: 'put',
      data
    })
  },

  // DELETE 请求
  delete(url, params = {}) {
    return service({
      url,
      method: 'delete',
      params
    })
  },

  // PATCH 请求
  patch(url, data = {}) {
    return service({
      url,
      method: 'patch',
      data
    })
  }
}

export default request

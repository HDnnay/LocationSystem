import request from './request'

// 认证相关API
export const login = (data) => {
  return request.post('/api/auth/login', data)
}

export const register = (data) => {
  return request.post('/api/auth/register', data)
}

export const refreshToken = (data) => {
  return request.post('/api/auth/refresh-token', data)
}

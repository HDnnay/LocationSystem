import request from './request'

// 认证相关API
export const login = (data) => {
  return request({
    url: '/api/auth/login',
    method: 'post',
    data
  })
}

export const register = (data) => {
  return request({
    url: '/api/auth/register',
    method: 'post',
    data
  })
}

export const refreshToken = (data) => {
  return request({
    url: '/api/auth/refresh-token',
    method: 'post',
    data
  })
}

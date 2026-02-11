import request from './request'

// 获取所有用户
export const getAllUsers = () => {
  return request.get('/api/users')
}

// 获取单个用户
export const getUserById = (id) => {
  return request.get(`/api/users/${id}`)
}

// 创建用户
export const createUser = (user) => {
  return request.post('/api/users', user)
}

// 更新用户
export const updateUser = (id, user) => {
  return request.put(`/api/users/${id}`, user)
}

// 删除用户
export const deleteUser = (id) => {
  return request.delete(`/api/users/${id}`)
}

// 分配角色给用户
export const assignRoles = (id, roleIds) => {
  return request.post(`/api/users/${id}/assign-roles`, roleIds)
}

import request from './request'

// 获取所有角色
export const getAllRoles = () => {
  return request.get('/api/roles')
}

// 获取单个角色
export const getRoleById = (id) => {
  return request.get(`/api/roles/${id}`)
}

// 创建角色
export const createRole = (role) => {
  return request.post('/api/roles', role)
}

// 更新角色
export const updateRole = (id, role) => {
  return request.put(`/api/roles/${id}`, role)
}

// 删除角色
export const deleteRole = (id) => {
  return request.delete(`/api/roles/${id}`)
}

// 禁用角色
export const disableRole = (id) => {
  return request.post(`/api/roles/${id}/disable`)
}

// 启用角色
export const enableRole = (id) => {
  return request.post(`/api/roles/${id}/enable`)
}

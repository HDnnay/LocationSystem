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

// 更新角色状态
export const updateRoleStatus = (roleStatus) => {
  return request.put('/api/role/Status', roleStatus)
}

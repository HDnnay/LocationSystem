import request from './request'

// 权限相关API
export const getUserMenus = () => {
  return request.get('/api/permissions/user-menus')
}

export const getUserPermissions = () => {
  return request.get('/api/permissions/user-permissions')
}

export const getPermissions = () => {
  return request.get('/api/permissions')
}

export const getPermissionTree = () => {
  return request.get('/api/permissions/tree')
}

export const getPermissionTreeWithCheckStatus = (roleId) => {
  return request.get('/api/permissions/tree-with-check', {
    params: { roleId }
  })
}

export const getRoles = () => {
  return request.get('/api/roles')
}

export const createPermission = (data) => {
  return request.post('/api/permissions', data)
}

export const updatePermission = (id, data) => {
  return request.put(`/api/permissions/${id}`, data)
}

export const deletePermission = (id) => {
  return request.delete(`/api/permissions/${id}`)
}

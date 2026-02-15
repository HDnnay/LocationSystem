import request from './request'

// 获取所有菜单
export const getAllMenus = (page = 1, pageSize = 10) => {
  return request.get('/api/menus', { params: { page, pageSize } })
}

// 获取菜单树形结构
export const getMenuTree = () => {
  return request.get('/api/menus/tree')
}

// 获取单个菜单
export const getMenuById = (id) => {
  return request.get(`/api/menus/${id}`)
}

// 创建菜单
export const createMenu = (menu) => {
  return request.post('/api/menus', menu)
}

// 更新菜单
export const updateMenu = (id, menu) => {
  return request.put(`/api/menus/${id}`, menu)
}

// 删除菜单
export const deleteMenu = (id) => {
  return request.delete(`/api/menus/${id}`)
}

// 为菜单分配权限
export const assignPermissionsToMenu = (menuId, permissionIds) => {
  return request.post(`/api/menus/${menuId}/permissions`, permissionIds)
}

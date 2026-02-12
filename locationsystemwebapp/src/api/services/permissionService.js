import request from '../request' // 导入请求实例

// 权限相关的 API 服务
const permissionService = {
  // 获取用户菜单权限
  getUserMenus() {
    return request.get('/permissions/user-menus')
  },

  // 获取所有权限列表
  getAllPermissions() {
    return request.get('/permissions')
  }
}

export default permissionService
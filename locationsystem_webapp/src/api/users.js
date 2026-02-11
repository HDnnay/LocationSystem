import request from './request'

const userApi = {
  // 获取所有用户
  getAllUsers: () => {
    return request({
      url: '/api/users',
      method: 'get'
    })
  },
  
  // 获取单个用户
  getUserById: (id) => {
    return request({
      url: `/api/users/${id}`,
      method: 'get'
    })
  },
  
  // 更新用户
  updateUser: (id, user) => {
    return request({
      url: `/api/users/${id}`,
      method: 'put',
      data: user
    })
  },
  
  // 删除用户
  deleteUser: (id) => {
    return request({
      url: `/api/users/${id}`,
      method: 'delete'
    })
  },
  
  // 分配角色给用户
  assignRoles: (id, roleIds) => {
    return request({
      url: `/api/users/${id}/assign-roles`,
      method: 'post',
      data: roleIds
    })
  }
}

export default userApi

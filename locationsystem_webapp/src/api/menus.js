import request from './request'

const menuApi = {
  // 获取所有菜单
  getAllMenus: (page = 1, pageSize = 10) => {
    return request({
      url: '/api/menus',
      method: 'get',
      params: {
        page,
        pageSize
      }
    })
  },

  // 获取菜单树形结构
  getMenuTree: () => {
    return request({
      url: '/api/menus/tree',
      method: 'get'
    })
  },

  // 获取单个菜单
  getMenuById: (id) => {
    return request({
      url: `/api/menus/${id}`,
      method: 'get'
    })
  },

  // 创建菜单
  createMenu: (menu) => {
    return request({
      url: '/api/menus',
      method: 'post',
      data: menu
    })
  },

  // 更新菜单
  updateMenu: (id, menu) => {
    return request({
      url: `/api/menus/${id}`,
      method: 'put',
      data: menu
    })
  },

  // 删除菜单
  deleteMenu: (id) => {
    return request({
      url: `/api/menus/${id}`,
      method: 'delete'
    })
  }
}

export default menuApi

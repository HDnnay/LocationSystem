import request from './request'

// 权限相关API
export const getUserMenus = () => {
    return request({
        url: '/api/permissions/user-menus',
        method: 'get'
    })
}

export const getPermissions = () => {
    return request({
        url: '/api/permissions',
        method: 'get'
    })
}

export const getPermissionTree = () => {
    return request({
        url: '/api/permissions/tree',
        method: 'get'
    })
}

export const createPermission = (data) => {
    return request({
        url: '/api/permissions',
        method: 'post',
        data
    })
}

export const updatePermission = (id, data) => {
    return request({
        url: `/api/permissions/${id}`,
        method: 'put',
        data
    })
}

export const deletePermission = (id) => {
    return request({
        url: `/api/permissions/${id}`,
        method: 'delete'
    })
}

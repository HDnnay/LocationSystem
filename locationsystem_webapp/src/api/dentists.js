import request from './request'

// 获取牙医列表
export const getDentists = (params) => {
  return request.get('/api/Dentists', { params })
}

// 创建牙医
export const createDentist = (data) => {
  return request.post('/api/Dentists', data)
}

// 更新牙医
export const updateDentist = (id, data) => {
  return request.put(`/api/Dentists/${id}`, data)
}

// 删除牙医
export const deleteDentist = (id) => {
  return request.delete(`/api/Dentists/${id}`)
}

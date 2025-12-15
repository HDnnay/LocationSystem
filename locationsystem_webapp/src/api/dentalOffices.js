import request from './request'

// 获取牙科诊所列表
export const getDentalOffices = (params) => {
  return request.get('/api/DentalOffices', { params })
}

// 创建牙科诊所
export const createDentalOffice = (data) => {
  return request.post('/api/DentalOffices', data)
}

// 获取牙科诊所详情
export const getDentalOffice = (id) => {
  return request.get(`/api/DentalOffices/${id}`)
}

// 更新牙科诊所
export const updateDentalOffice = (id, data) => {
  return request.put(`/api/DentalOffices/${id}`, data)
}

// 删除牙科诊所
export const deleteDentalOffice = (id) => {
  return request.delete(`/api/DentalOffices/${id}`)
}

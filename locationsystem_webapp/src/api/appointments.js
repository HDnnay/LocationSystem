import request from './request'

// 获取预约列表
export const getAppointments = (params) => {
  return request.get('/api/Appointments', { params })
}

// 创建预约
export const createAppointment = (data) => {
  console.log(data);
  return request.post('/api/Appointments', data)
}

// 获取预约详情
export const getAppointment = (id) => {
  return request.get(`/api/Appointments/${id}`)
}

// 更新预约
export const updateAppointment = (id, data) => {
  return request.put(`/api/Appointments/${id}`, data)
}

// 删除预约
export const deleteAppointment = (id) => {
  return request.delete(`/api/Appointments/${id}`)
}

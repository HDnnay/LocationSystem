import request from './request'

// 获取患者列表
export const getPatients = (params) => {
  return request.get('/api/Patients', { params })
}

// 创建患者
export const createPatient = (data) => {
  return request.post('/api/Patients', data)
}

// 获取患者详情
export const getPatient = (id) => {
  return request.get(`/api/Patients/${id}`)
}

// 更新患者
export const updatePatient = (id, data) => {
  return request.put(`/api/Patients/${id}`, data)
}

// 删除患者
export const deletePatient = (id) => {
  return request.delete(`/api/Patients/${id}`)
}

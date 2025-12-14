import request from '@/api/request'

const appointmentService = {
  // 获取所有预约
  getAll(params = {}) {
    return request.get('/api/appointments', params)
  },

  // 获取单个预约
  getById(id) {
    return request.get(`/api/appointments/${id}`)
  },

  // 创建预约
  create(data) {
    return request.post('/api/appointments', data)
  },

  // 更新预约
  update(id, data) {
    return request.put(`/api/appointments/${id}`, data)
  },

  // 删除预约
  delete(id) {
    return request.delete(`/api/appointments/${id}`)
  }
}

export default appointmentService

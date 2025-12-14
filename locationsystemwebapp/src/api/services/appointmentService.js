import request from '@/api/request'

const appointmentService = {
  // 获取所有预约
  getAll(params = {}) {
    console.log('准备请求预约列表API，参数:', params)
    return request.get('/appointments', params)
  },

  // 获取单个预约
  getById(id) {
    return request.get(`/appointments/${id}`)
  },

  // 创建预约
  create(data) {
    return request.post('/appointments', data)
  },

  // 更新预约
  update(id, data) {
    return request.put(`/appointments/${id}`, data)
  },

  // 删除预约
  delete(id) {
    return request.delete(`/appointments/${id}`)
  }
}

export default appointmentService

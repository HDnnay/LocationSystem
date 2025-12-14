import request from '@/api/request'

const dentalOfficeService = {
  // 获取所有牙科诊所
  getAll(params = {}) {
    return request.get('/dentaloffices', params)
  },

  // 获取单个牙科诊所
  getById(id) {
    return request.get(`/dentaloffices/${id}`)
  },

  // 创建牙科诊所
  create(data) {
    return request.post('/dentaloffices', data)
  },

  // 更新牙科诊所
  update(id, data) {
    return request.put(`/dentaloffices/${id}`, data)
  },

  // 删除牙科诊所
  delete(id) {
    return request.delete(`/dentaloffices/${id}`)
  }
}

export default dentalOfficeService

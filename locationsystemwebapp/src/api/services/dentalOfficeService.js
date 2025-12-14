import request from '@/api/request'

const dentalOfficeService = {
  // 获取所有牙科诊所
  getAll(params = {}) {
    return request.get('/api/dentaloffices', params)
  },

  // 获取单个牙科诊所
  getById(id) {
    return request.get(`/api/dentaloffices/${id}`)
  },

  // 创建牙科诊所
  create(data) {
    return request.post('/api/dentaloffices', data)
  },

  // 更新牙科诊所
  update(id, data) {
    return request.put(`/api/dentaloffices/${id}`, data)
  },

  // 删除牙科诊所
  delete(id) {
    return request.delete(`/api/dentaloffices/${id}`)
  }
}

export default dentalOfficeService

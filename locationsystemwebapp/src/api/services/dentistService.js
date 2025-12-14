import request from '@/api/request'

const dentistService = {
  // 获取所有牙医
  getAll(params = {}) {
    return request.get('/api/dentists', params)
  },

  // 获取单个牙医
  getById(id) {
    return request.get(`/api/dentists/${id}`)
  },

  // 创建牙医
  create(data) {
    return request.post('/api/dentists', data)
  },

  // 更新牙医
  update(id, data) {
    return request.put(`/api/dentists/${id}`, data)
  },

  // 删除牙医
  delete(id) {
    return request.delete(`/api/dentists/${id}`)
  }
}

export default dentistService

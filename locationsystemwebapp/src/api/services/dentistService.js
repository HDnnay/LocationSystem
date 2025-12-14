import request from '@/api/request'

const dentistService = {
  // 获取所有牙医
  getAll(params = {}) {
    return request.get('/dentists', params)
  },

  // 获取单个牙医
  getById(id) {
    return request.get(`/dentists/${id}`)
  },

  // 创建牙医
  create(data) {
    return request.post('/dentists', data)
  },

  // 更新牙医
  update(id, data) {
    return request.put(`/dentists/${id}`, data)
  },

  // 删除牙医
  delete(id) {
    return request.delete(`/dentists/${id}`)
  }
}

export default dentistService

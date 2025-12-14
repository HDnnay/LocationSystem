import request from '@/api/request'

const patientService = {
  // 获取所有患者
  getAll(params = {}) {
    return request.get('/patients', params)
  },

  // 获取单个患者
  getById(id) {
    return request.get(`/patients/${id}`)
  },

  // 创建患者
  create(data) {
    return request.post('/patients', data)
  },

  // 更新患者
  update(id, data) {
    return request.put(`/patients/${id}`, data)
  },

  // 删除患者
  delete(id) {
    return request.delete(`/patients/${id}`)
  }
}

export default patientService

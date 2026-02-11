import request from './request'

// 获取公司列表
export const getCompanies = (params) => {
  return request.get('/api/Company', { params })
}

// 获取省份公司数据
export const getProviceCompany = () => {
  return request.get('/api/company/get/provice')
}

// 获取公司详情
export const getCompanyById = (id) => {
  return request.get(`/api/companies/${id}`)
}

// 创建公司
export const createCompany = (data) => {
  return request.post('/api/companies', data)
}

// 更新公司
export const updateCompany = (id, data) => {
  return request.put(`/api/companies/${id}`, data)
}

// 删除公司
export const deleteCompany = (id) => {
  return request.delete(`/api/companies/${id}`)
}

// 按关键字搜索公司
export const searchCompanies = (keyword, options = {}) => {
  const params = Object.assign({}, options, { q: keyword })
  return getCompanies(params)
}

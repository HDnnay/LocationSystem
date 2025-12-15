// 导入request工具
import request from '@/utils/request';

// 患者API模块
const patientsApi = {
  // 获取患者列表
  getPatients: (params) => {
    return request({
      url: '/api/patients',
      method: 'get',
      params
    });
  },

  // 获取单个患者详情
  getPatient: (id) => {
    return request({
      url: `/api/patients/${id}`,
      method: 'get'
    });
  },

  // 创建患者
  createPatient: (data) => {
    return request({
      url: '/api/patients',
      method: 'post',
      data
    });
  },

  // 更新患者
  updatePatient: (id, data) => {
    return request({
      url: `/api/patients/${id}`,
      method: 'put',
      data
    });
  },

  // 删除患者
  deletePatient: (id) => {
    return request({
      url: `/api/patients/${id}`,
      method: 'delete'
    });
  }
};

export default patientsApi;
// 导入request工具
import request from '@/utils/request';

// 牙科诊所API模块
const dentalOfficesApi = {
  // 获取牙科诊所列表
  getDentalOffices: (params) => {
    return request({
      url: '/api/dentaloffices',
      method: 'get',
      params
    });
  },

  // 获取单个牙科诊所详情
  getDentalOffice: (id) => {
    return request({
      url: `/api/dentaloffices/${id}`,
      method: 'get'
    });
  },

  // 创建牙科诊所
  createDentalOffice: (data) => {
    return request({
      url: '/api/dentaloffices',
      method: 'post',
      data
    });
  },

  // 更新牙科诊所
  updateDentalOffice: (id, data) => {
    return request({
      url: `/api/dentaloffices/${id}`,
      method: 'put',
      data
    });
  },

  // 删除牙科诊所
  deleteDentalOffice: (id) => {
    return request({
      url: `/api/dentaloffices/${id}`,
      method: 'delete'
    });
  }
};

export default dentalOfficesApi;
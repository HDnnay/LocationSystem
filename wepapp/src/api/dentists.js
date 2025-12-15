// 导入request工具
import request from '@/utils/request';

// 牙医API模块
const dentistsApi = {
  // 获取牙医列表
  getDentists: (params) => {
    return request({
      url: '/api/dentists',
      method: 'get',
      params
    });
  },

  // 获取单个牙医详情
  getDentist: (id) => {
    return request({
      url: `/api/dentists/${id}`,
      method: 'get'
    });
  },

  // 创建牙医
  createDentist: (data) => {
    return request({
      url: '/api/dentists',
      method: 'post',
      data
    });
  },

  // 更新牙医
  updateDentist: (id, data) => {
    return request({
      url: `/api/dentists/${id}`,
      method: 'put',
      data
    });
  },

  // 删除牙医
  deleteDentist: (id) => {
    return request({
      url: `/api/dentists/${id}`,
      method: 'delete'
    });
  }
};

export default dentistsApi;
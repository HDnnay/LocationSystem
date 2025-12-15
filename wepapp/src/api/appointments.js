// 导入request工具
import request from '@/utils/request';

// 预约API模块
const appointmentsApi = {
  // 获取预约列表
  getAppointments: (params) => {
    return request({
      url: '/api/appointments',
      method: 'get',
      params
    });
  },

  // 获取单个预约详情
  getAppointment: (id) => {
    return request({
      url: `/api/appointments/${id}`,
      method: 'get'
    });
  },

  // 创建预约
  createAppointment: (data) => {
    return request({
      url: '/api/appointments',
      method: 'post',
      data
    });
  },

  // 更新预约
  updateAppointment: (id, data) => {
    return request({
      url: `/api/appointments/${id}`,
      method: 'put',
      data
    });
  },

  // 删除预约
  deleteAppointment: (id) => {
    return request({
      url: `/api/appointments/${id}`,
      method: 'delete'
    });
  }
};

export default appointmentsApi;
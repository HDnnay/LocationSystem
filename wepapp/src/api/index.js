// API接口封装入口文件
// 导入request工具
import request from '@/utils/request';

// 导入各个API模块
import dentistsApi from './dentists';
import patientsApi from './patients';
import dentalOfficesApi from './dentalOffices';
import appointmentsApi from './appointments';

// 导出所有API模块
export default {
  dentists: dentistsApi,
  patients: patientsApi,
  dentalOffices: dentalOfficesApi,
  appointments: appointmentsApi
};

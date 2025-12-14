// API接口封装入口文件
// 导入request工具
import request from '@utils/request';

// 导入各个API模块
import exampleApi from './example';
import locationApi from './locationApi';

// 导出所有API模块
export default {
  example: exampleApi,
  location: locationApi
};

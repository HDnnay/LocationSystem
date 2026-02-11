import request from './request';
import { compressImages as compressImageUtil, formatFileSize } from '@/utils/imageProcessor';

// 获取租房列表（支持分页与筛选）
export const fetchRentHouses = (params) => {
  console.log("租房借口获取：");
  console.log(params);
  return request.get('/api/renthouse', { params });
};

// 上传房间图片
export const uploadRoomImage = async (files, onProgress = null, compressOptions = {}) => {
  // 压缩图片
  const compressionResult = await compressImageUtil(files, compressOptions);

  const formData = new FormData();
  //console.log("压缩前总大小:", formatFileSize(compressionResult.totalOriginalSize));
  //console.log("压缩后总大小:", formatFileSize(compressionResult.totalCompressedSize));
  //console.log("整体压缩率:", compressionResult.overallCompressionRatio + '%');
  //console.log("\n详细信息:");

  // 添加所有文件到 FormData
  compressionResult.files.forEach((file, index) => {
    const stat = compressionResult.stats[index];
    console.log(`${file.name}:`);
    console.log(`  压缩前: ${formatFileSize(stat.originalSize)}`);
    if (stat.skipped) {
      console.log(`  状态: 跳过压缩 (${stat.reason})`);
      console.log(`  压缩后: ${formatFileSize(stat.compressedSize)}`);
      console.log(`  压缩率: ${stat.compressionRatio}%`);
    } else {
      console.log(`  压缩后: ${formatFileSize(stat.compressedSize)}`);
      console.log(`  压缩率: ${stat.compressionRatio}%`);
    }
    formData.append('files', file);
  });

  console.log("\n上传中...");
  return request({
    url: '/api/RentHouse/upload-multiple',
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data'
    },
    responseType: 'json', // 明确指定响应类型
    onUploadProgress: onProgress
  });
};

// 获取单个租房详情
export const getRentHouse = (id) => {
  return request.get(`/api/renthouse/${id}`);
};

// 创建新的租房记录
export const createRentHouse = (data) => {
  return request.post('/api/renthouse', data);
};

// 更新租房记录
export const updateRentHouse = (id, data) => {
  return request.put(`/api/renthouse/${id}`, data);
};

// 删除租房记录
export const deleteRentHouse = (id) => {
  return request.delete(`/api/renthouse/${id}`);
};

// 更改租房状态（例如：上架/下架/已租）
export const changeRentHouseStatus = (id, payload) => {
  return request.patch(`/api/renthouse/${id}/status`, payload);
};

// 批量删除或批量操作
export const bulkOperateRentHouses = (ids, options = {}) => {
  return request.post('/api/renthouse/bulk', {
    ids,
    ...options,
  });
};

// 搜索租房（更灵活的搜索接口）
export const searchRentHouses = (query) => {
  return request.get('/api/renthouse/search', { params: query });
};

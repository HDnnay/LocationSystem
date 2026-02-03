// GitHub Copilot
// 租房相关的 API 封装
// 约定：项目中应存在一个统一的请求实例 `@/utils/request`（基于 axios）
// 如果项目使用不同路径，请调整 import 路径以匹配项目结构。

import request from '@/api/request';
import { compressImages as compressImageUtil, formatFileSize } from '@/utils/imageProcessor';
/**
 * 获取租房列表（支持分页与筛选）
 * @param {Object} params - 查询参数（如 page, pageSize, status, keyword 等）
 * @returns {Promise}
 */
export function fetchRentHouses(params) {
  console.log("租房借口获取：");
  console.log(params);
  return request({
    url: '/api/renthouse',
    method: 'get',
    params,
  });
}
export async function uploadRoomImage(files, onProgress = null, compressOptions = {}) {
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
}
/**
 * 获取单个租房详情
 * @param {string|number} id
 * @returns {Promise}
 */
export function getRentHouse(id) {
  return request({
    url: `/api/renthouse/${id}`,
    method: 'get',
  });
}

/**
 * 创建新的租房记录
 * @param {Object} data - 租房数据对象
 * @returns {Promise}
 */
export function createRentHouse(data) {
  return request({
    url: '/api/renthouse',
    method: 'post',
    data,
  });
}

/**
 * 更新租房记录
 * @param {string|number} id
 * @param {Object} data - 要更新的字段
 * @returns {Promise}
 */
export function updateRentHouse(id, data) {
  return request({
    url: `/api/renthouse/${id}`,
    method: 'put',
    data,
  });
}

/**
 * 删除租房记录
 * @param {string|number} id
 * @returns {Promise}
 */
export function deleteRentHouse(id) {
  return request({
    url: `/api/renthouse/${id}`,
    method: 'delete',
  });
}

/**
 * 更改租房状态（例如：上架/下架/已租）
 * @param {string|number} id
 * @param {Object} payload - 包含状态或其他控制字段，如 { status: 'rented' }
 * @returns {Promise}
 */
export function changeRentHouseStatus(id, payload) {
  return request({
    url: `/api/renthouse/${id}/status`,
    method: 'patch',
    data: payload,
  });
}

/**
 * 批量删除或批量操作
 * @param {Array<string|number>} ids
 * @param {Object} options - 可选参数，决定执行的批量操作
 * @returns {Promise}
 */
export function bulkOperateRentHouses(ids, options = {}) {
  return request({
    url: '/api/renthouse/bulk',
    method: 'post',
    data: {
      ids,
      ...options,
    },
  });
}

/**
 * 搜索租房（更灵活的搜索接口）
 * @param {Object} query - 搜索条件
 * @returns {Promise}
 */
export function searchRentHouses(query) {
  return request({
    url: '/api/renthouse/search',
    method: 'get',
    params: query,
  });
}

export default {
  fetchRentHouses,
  getRentHouse,
  createRentHouse,
  updateRentHouse,
  deleteRentHouse,
  changeRentHouseStatus,
  bulkOperateRentHouses,
  searchRentHouses,
  uploadRoomImage
};

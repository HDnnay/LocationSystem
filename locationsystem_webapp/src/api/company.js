// 公司相关 API 封装
// 约定：后端返回 JSON，鉴权 token 存于 localStorage.authToken

const BASE_URL =
  (typeof process !== 'undefined' && process.env.REACT_APP_API_BASE_URL) ||
  (typeof window !== 'undefined' && window.__API_BASE_URL__) ||
  '/api';

/**
 * 构建带查询字符串的 URL
 * @param {string} path
 * @param {Object} [params]
 * @returns {string}
 */
function buildUrl(path, params) {
  if (!params || Object.keys(params).length === 0) {
    return `${BASE_URL}${path}`;
  }
  const esc = encodeURIComponent;
  const query = Object.keys(params)
    .filter((k) => params[k] !== undefined && params[k] !== null)
    .map((k) => `${esc(k)}=${esc(params[k])}`)
    .join('&');
  return `${BASE_URL}${path}?${query}`;
}

/**
 * 获取鉴权头
 * @returns {Object}
 */
function getAuthHeaders() {
  try {
    const token =
      (typeof localStorage !== 'undefined' && localStorage.getItem('authToken')) || '';
    return token ? { Authorization: `Bearer ${token}` } : {};
  } catch {
    return {};
  }
}

/**
 * 通用请求包装，抛出在非 2xx 时的错误
 * @param {string} url
 * @param {RequestInit} options
 */
async function request(url, options = {}) {
  const headers = Object.assign({ 'Content-Type': 'application/json' }, getAuthHeaders(), options.headers || {});
  const opts = Object.assign({}, options, { headers });
  const res = await fetch(url, opts);
  const contentType = res.headers.get('content-type') || '';
  const isJson = contentType.includes('application/json');

  if (!res.ok) {
    const body = isJson ? await res.json().catch(() => null) : await res.text().catch(() => null);
    const message = (body && body.message) || res.statusText || 'Request failed';
    const error = new Error(message);
    error.status = res.status;
    error.body = body;
    throw error;
  }

  if (res.status === 204) {
    return null;
  }

  return isJson ? res.json() : res.text();
}

/**
 * 获取公司列表
 * @param {Object} [params] - 支持分页、筛选等查询参数
 * @returns {Promise<Object>}
 */
export async function getCompanies(params) {
  const url = buildUrl('/Company', params);
  console.log(url)
  return request(url, { method: 'GET' });
}
export async function getProviceCompany() {
  const url = buildUrl("/company/get/provice", null);
  console.log(url);
  return request(url, { method: 'GET' });
}
/**
 * 根据 id 获取公司详情
 * @param {string|number} id
 * @returns {Promise<Object>}
 */
export async function getCompanyById(id) {
  if (id === undefined || id === null) {
    throw new Error('getCompanyById: id 不能为空');
  }
  const url = buildUrl(`/companies/${encodeURIComponent(id)}`);
  return request(url, { method: 'GET' });
}

/**
 * 创建公司
 * @param {Object} data
 * @returns {Promise<Object>}
 */
export async function createCompany(data) {
  const url = buildUrl('/companies');
  return request(url, { method: 'POST', body: JSON.stringify(data) });
}

/**
 * 更新公司
 * @param {string|number} id
 * @param {Object} data
 * @returns {Promise<Object>}
 */
export async function updateCompany(id, data) {
  if (id === undefined || id === null) {
    throw new Error('updateCompany: id 不能为空');
  }
  const url = buildUrl(`/companies/${encodeURIComponent(id)}`);
  return request(url, { method: 'PUT', body: JSON.stringify(data) });
}

/**
 * 删除公司
 * @param {string|number} id
 * @returns {Promise<null>}
 */
export async function deleteCompany(id) {
  if (id === undefined || id === null) {
    throw new Error('deleteCompany: id 不能为空');
  }
  const url = buildUrl(`/companies/${encodeURIComponent(id)}`);
  return request(url, { method: 'DELETE' });
}

/**
 * 按关键字搜索公司（示例封装）
 * @param {string} keyword
 * @param {Object} [options]
 * @returns {Promise<Object>}
 */
export async function searchCompanies(keyword, options = {}) {
  const params = Object.assign({}, options, { q: keyword });
  return getCompanies(params);
}

export default {
  getCompanies,
  getCompanyById,
  createCompany,
  updateCompany,
  deleteCompany,
  searchCompanies,
};

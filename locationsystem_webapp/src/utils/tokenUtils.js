// Token 工具函数

/**
 * 解析JWT token，获取payload
 * @param {string} token - JWT token
 * @returns {object} token payload
 */
export const parseToken = (token) => {
  if (!token) return null

  try {
    const payloadBase64 = token.split('.')[1]
    const payloadJson = atob(payloadBase64)
    return JSON.parse(payloadJson)
  } catch (error) {
    console.error('解析token失败:', error)
    return null
  }
}

/**
 * 检查token是否过期
 * @param {string} token - JWT token
 * @param {number} bufferMinutes - 缓冲时间（分钟），默认5分钟
 * @returns {boolean} 是否已过期
 */
export const isTokenExpired = (token, bufferMinutes = 5) => {
  const payload = parseToken(token)
  if (!payload || !payload.exp) return true

  const now = Date.now() / 1000
  const exp = payload.exp
  const buffer = bufferMinutes * 60

  return now > exp - buffer
}

/**
 * 检查是否需要刷新token
 * @param {number} bufferMinutes - 缓冲时间（分钟），默认5分钟
 * @returns {boolean} 是否需要刷新
 */
export const needRefreshToken = (bufferMinutes = 5) => {
  const token = localStorage.getItem('access_token')
  if (!token) return false

  return isTokenExpired(token, bufferMinutes)
}

/**
 * 获取token过期时间
 * @param {string} token - JWT token
 * @returns {Date|null} 过期时间
 */
export const getTokenExpiration = (token) => {
  const payload = parseToken(token)
  if (!payload || !payload.exp) return null

  return new Date(payload.exp * 1000)
}

/**
 * 清除所有认证信息
 */
export const clearAuthInfo = () => {
  localStorage.removeItem('access_token')
  localStorage.removeItem('refresh_token')
  localStorage.removeItem('user_type')
  localStorage.removeItem('user_info')
}

/**
 * 保存认证信息
 * @param {object} authData - 认证数据
 */
export const saveAuthInfo = (authData) => {
  if (authData.accessToken) {
    localStorage.setItem('access_token', authData.accessToken)
  }
  if (authData.refreshToken) {
    localStorage.setItem('refresh_token', authData.refreshToken)
  }
  if (authData.userInfo) {
    localStorage.setItem('user_info', JSON.stringify(authData.userInfo))
  }
}

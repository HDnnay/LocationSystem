// 认证存储管理

import storageService from './storageService'

/**
 * 认证存储管理
 */
class AuthStorage {
  /**
   * 保存认证信息
   * @param {object} authData 认证数据
   */
  saveAuthData(authData) {
    if (authData.accessToken) {
      storageService.set('access_token', authData.accessToken)
    }
    if (authData.refreshToken) {
      storageService.set('refresh_token', authData.refreshToken)
    }
    if (authData.userInfo) {
      storageService.set('user_info', authData.userInfo)
      // 保存user_type
      if (authData.userInfo.userType) {
        storageService.set('user_type', authData.userInfo.userType)
      }
    }
  }

  /**
   * 获取访问令牌
   * @returns {string|null} 访问令牌
   */
  getAccessToken() {
    return storageService.get('access_token')
  }

  /**
   * 获取刷新令牌
   * @returns {string|null} 刷新令牌
   */
  getRefreshToken() {
    return storageService.get('refresh_token')
  }

  /**
   * 获取用户信息
   * @returns {object|null} 用户信息
   */
  getUserInfo() {
    return storageService.get('user_info')
  }

  /**
   * 获取用户类型
   * @returns {string|null} 用户类型
   */
  getUserType() {
    return storageService.get('user_type')
  }

  /**
   * 清除认证信息
   */
  clearAuthData() {
    storageService.remove('access_token')
    storageService.remove('refresh_token')
    storageService.remove('user_info')
    storageService.remove('user_type')
  }

  /**
   * 检查是否已认证
   * @returns {boolean} 是否已认证
   */
  isAuthenticated() {
    return !!this.getAccessToken()
  }
}

// 导出单例
const authStorage = new AuthStorage()
export default authStorage

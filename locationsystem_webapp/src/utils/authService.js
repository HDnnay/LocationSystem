// 认证服务，统一管理token的刷新和存储

import api from '../api'
import authStorage from './authStorage'
import router from '../router'

class AuthService {
  constructor() {
    this.isRefreshing = false
    this.refreshSubscribers = []
  }

  /**
   * 刷新token
   * @returns {Promise<string>} 新的accessToken
   */
  async refreshToken() {
    if (this.isRefreshing) {
      // 正在刷新token，返回一个Promise等待刷新完成
      return new Promise(resolve => {
        this.refreshSubscribers.push(resolve)
      })
    }

    this.isRefreshing = true

    try {
      const refreshToken = authStorage.getRefreshToken()
      const userType = authStorage.getUserType()

      if (!refreshToken || !userType) {
        throw new Error('缺少refreshToken或userType')
      }

      const response = await api.auth.refreshToken({
        RefreshToken: refreshToken
      })

      // 检查响应状态码
      if (response.status === 200) {
        const res = response.data

        if (!res.accessToken) {
          throw new Error('刷新token失败，未返回accessToken')
        }

        // 保存新的token信息
        authStorage.saveAuthData({
          accessToken: res.accessToken,
          refreshToken: res.refreshToken,
          userInfo: res.userInfo
        })

        // 通知所有订阅者
        this.notifySubscribers(res.accessToken)

        return res.accessToken
      } else {
        throw new Error(`刷新token失败，状态码: ${response.status}`)
      }
    } catch (error) {
      console.error('刷新token失败:', error)
      // 刷新失败，清除认证信息并跳转到登录页
      authStorage.clearAuthData()
      router.push('/login')
      throw error
    } finally {
      this.isRefreshing = false
    }
  }

  /**
   * 通知所有订阅者
   * @param {string} newToken 新的accessToken
   */
  notifySubscribers(newToken) {
    this.refreshSubscribers.forEach(callback => callback(newToken))
    this.refreshSubscribers = []
  }

  /**
   * 检查是否正在刷新token
   * @returns {boolean} 是否正在刷新token
   */
  getIsRefreshing() {
    return this.isRefreshing
  }
}

// 导出单例
const authService = new AuthService()
export default authService

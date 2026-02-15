// 本地存储管理服务

/**
 * 本地存储管理服务
 */
class StorageService {
  constructor() {
    this.prefix = 'location_system_'
  }

  /**
   * 设置存储项
   * @param {string} key 存储键名
   * @param {any} value 存储值
   */
  set(key, value) {
    try {
      const storedValue = JSON.stringify(value)
      localStorage.setItem(this.prefix + key, storedValue)
    } catch (error) {
      console.error('设置本地存储失败:', error)
    }
  }

  /**
   * 获取存储项
   * @param {string} key 存储键名
   * @param {any} defaultValue 默认值
   * @returns {any} 存储值或默认值
   */
  get(key, defaultValue = null) {
    try {
      const storedValue = localStorage.getItem(this.prefix + key)
      return storedValue ? JSON.parse(storedValue) : defaultValue
    } catch (error) {
      console.error('获取本地存储失败:', error)
      return defaultValue
    }
  }

  /**
   * 删除存储项
   * @param {string} key 存储键名
   */
  remove(key) {
    try {
      localStorage.removeItem(this.prefix + key)
    } catch (error) {
      console.error('删除本地存储失败:', error)
    }
  }

  /**
   * 清空所有存储项
   */
  clear() {
    try {
      // 只清空带有前缀的存储项
      Object.keys(localStorage).forEach(key => {
        if (key.startsWith(this.prefix)) {
          localStorage.removeItem(key)
        }
      })
    } catch (error) {
      console.error('清空本地存储失败:', error)
    }
  }

  /**
   * 检查存储项是否存在
   * @param {string} key 存储键名
   * @returns {boolean} 是否存在
   */
  has(key) {
    return localStorage.getItem(this.prefix + key) !== null
  }
}

// 导出单例
const storageService = new StorageService()
export default storageService

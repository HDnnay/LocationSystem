// 菜单管理工具类
import { ref } from 'vue';
import * as permissionsApi from '../api/permissions';

class MenuManager {
  constructor() {
    this.userId = this.getUserId();
    this.cacheKey = `user_menus_${this.userId}`;
    this.metaKey = `${this.cacheKey}_meta`;
    this.hubConnection = null;
    this.isConnected = ref(false);
    // 后端API地址，优先从环境变量获取，否则使用相对路径
    this.backendUrl = import.meta.env.VITE_BACKEND_URL || '';
    // 缓存版本，用于处理数据结构变化
    this.cacheVersion = 'v1';
  }

  // 获取用户ID，确保一致性
  getUserId() {
    try {
      const userInfo = JSON.parse(localStorage.getItem('user_info') || '{}');
      if (userInfo && userInfo.id) {
        // 同时更新userId到localStorage，确保一致性
        localStorage.setItem('userId', userInfo.id);
        return userInfo.id;
      }
    } catch (error) {
      console.error('解析用户信息失败:', error);
    }
    // 尝试直接从localStorage获取userId
    const userId = localStorage.getItem('userId');
    if (userId) {
      return userId;
    }
    // 默认值
    return 'guest';
  }

  // 初始化SignalR连接
  initSignalR() {
    console.log('初始化SignalR连接');
    if (this.hubConnection) return;
    console.log('动态导入SignalR客户端库');

    // 动态导入SignalR客户端库
    import('@microsoft/signalr').then((signalR) => {
      // 构建SignalR连接URL
      let hubUrl = '';
      if (this.backendUrl && this.backendUrl.includes('backend')) {
        // 容器环境，使用localhost地址
        hubUrl = 'http://localhost:8000/hub/menu';
        console.log('SignalR连接URL:', hubUrl);
      } else if (this.backendUrl) {
        // 其他环境，使用配置的后端地址
        hubUrl = `${this.backendUrl}/hub/menu`;
        console.log('SignalR连接URL:', hubUrl);
      } else {
        // 默认使用相对路径
        hubUrl = '/hub/menu';
        console.log('SignalR连接URL:', hubUrl);
      }

      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect()
        .build();

      // 监听菜单更新事件
      this.hubConnection.on('MenuUpdated', () => {
        console.log('菜单已更新，清除缓存');
        this.clearCache();
        // 触发菜单刷新事件
        window.dispatchEvent(new CustomEvent('menu:updated'));
      });

      // 监听连接状态
      this.hubConnection.onclose(() => {
        this.isConnected.value = false;
        console.log('SignalR连接已关闭');
      });

      // 开始连接
      this.hubConnection.start()
        .then(() => {
          this.isConnected.value = true;
          console.log('SignalR连接已建立');
          // 加入用户组
          this.hubConnection.invoke('JoinUserGroup', this.userId)
            .catch(err => console.error('加入用户组失败:', err));
        })
        .catch(err => console.error('SignalR连接失败:', err));
    });
  }

  // 获取菜单，支持重试机制
  async getMenus(retryCount = 0, maxRetries = 2) {
    // 检查本地缓存是否有效
    if (this.isCacheValid()) {
      console.log('从缓存获取菜单');
      try {
        const cachedMenus = this.getFromCache();
        if (Array.isArray(cachedMenus)) {
          return cachedMenus;
        }
        // 缓存数据不是数组，清除后重新获取
        this.clearCache();
      } catch (error) {
        console.error('从缓存获取菜单失败:', error);
        // 缓存损坏，清除后重新获取
        this.clearCache();
      }
    }

    // 缓存无效，从后端获取
    try {
      console.log('从后端获取菜单');
      const menus = await permissionsApi.getUserMenus();
      console.log('获取菜单成功:', menus);

      // 确保返回的是数组
      if (Array.isArray(menus)) {
        this.saveToCache(menus);
        return menus;
      } else {
        console.error('后端返回的菜单数据不是数组:', menus);
        // 尝试从缓存获取，即使已过期
        const cachedMenus = this.getFromCache(true);
        if (Array.isArray(cachedMenus)) {
          console.log('从过期缓存获取菜单');
          return cachedMenus;
        }
      }
    } catch (error) {
      console.error('获取菜单失败:', error);
      console.error('错误详情:', error.response);

      // 尝试从缓存获取，即使已过期
      const cachedMenus = this.getFromCache(true);
      if (Array.isArray(cachedMenus)) {
        console.log('从过期缓存获取菜单');
        return cachedMenus;
      }
    }

    // 重试机制
    if (retryCount < maxRetries) {
      console.log(`重试获取菜单 (${retryCount + 1}/${maxRetries})`);
      // 延迟1秒后重试
      await new Promise(resolve => setTimeout(resolve, 1000));
      return this.getMenus(retryCount + 1, maxRetries);
    }

    console.log('无缓存，返回空菜单');
    return [];
  }

  // 检查缓存是否有效
  isCacheValid() {
    const meta = localStorage.getItem(this.metaKey);
    if (!meta) return false;

    try {
      const { expireAt, version } = JSON.parse(meta);
      // 检查版本和过期时间
      return version === this.cacheVersion && new Date() < new Date(expireAt);
    } catch (error) {
      console.error('解析缓存元数据失败:', error);
      return false;
    }
  }

  // 从缓存获取菜单，支持忽略过期时间
  getFromCache(ignoreExpire = false) {
    try {
      const menus = localStorage.getItem(this.cacheKey);
      if (!menus) return null;

      if (!ignoreExpire && !this.isCacheValid()) {
        return null;
      }

      return JSON.parse(menus);
    } catch (error) {
      console.error('从缓存获取菜单失败:', error);
      // 缓存损坏，清除缓存
      this.clearCache();
      return null;
    }
  }

  // 保存到缓存
  saveToCache(menus) {
    // 检查menus是否存在
    if (!menus) {
      console.warn('菜单数据为空，跳过缓存');
      return;
    }

    const now = new Date();
    const expireAt = new Date(now.getTime() + 30 * 60 * 1000); // 30分钟过期

    try {
      // 检查localStorage容量
      const menusStr = JSON.stringify(menus);
      const metaStr = JSON.stringify({
        lastModified: now.toISOString(),
        expireAt: expireAt.toISOString(),
        version: this.cacheVersion
      });

      // 简单的容量检查
      if (menusStr && metaStr && (menusStr.length + metaStr.length > 5 * 1024 * 1024)) { // 5MB限制
        console.warn('菜单数据超过localStorage容量限制，跳过缓存');
        return;
      }

      localStorage.setItem(this.cacheKey, menusStr);
      localStorage.setItem(this.metaKey, metaStr);
    } catch (error) {
      console.error('保存菜单缓存失败:', error);
      // 可能是容量限制或其他错误，清除缓存
      this.clearCache();
    }
  }

  // 清除缓存
  clearCache() {
    try {
      localStorage.removeItem(this.cacheKey);
      localStorage.removeItem(this.metaKey);
    } catch (error) {
      console.error('清除菜单缓存失败:', error);
    }
  }

  // 强制刷新菜单
  async refreshMenus() {
    this.clearCache();
    return this.getMenus();
  }

  // 断开SignalR连接
  disconnectSignalR() {
    if (this.hubConnection) {
      this.hubConnection.stop()
        .then(() => {
          this.isConnected.value = false;
          console.log('SignalR连接已断开');
        })
        .catch(err => console.error('断开SignalR连接失败:', err));
    }
  }
}

// 导出单例实例
const menuManager = new MenuManager();
export default menuManager;

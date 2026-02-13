// 菜单管理工具类
import { ref } from 'vue';
import * as permissionsApi from '../api/permissions';

class MenuManager {
  constructor() {
    // 从user_info中获取userId
    let userId = 'guest';
    try {
      const userInfo = JSON.parse(localStorage.getItem('user_info') || '{}');
      if (userInfo && userInfo.id) {
        userId = userInfo.id;
      }
    } catch (error) {
      console.error('解析用户信息失败:', error);
    }
    this.cacheKey = `user_menus_${userId}`;
    this.metaKey = `${this.cacheKey}_meta`;
    this.hubConnection = null;
    this.isConnected = ref(false);
    // 后端API地址，优先从环境变量获取，否则使用相对路径
    this.backendUrl = import.meta.env.VITE_BACKEND_URL || '';
  }

  // 初始化SignalR连接
  initSignalR() {
    console.log('初始化SignalR连接');
    if (this.hubConnection) return;
    console.log('动态导入SignalR客户端库');

    // 动态导入SignalR客户端库
    import('@microsoft/signalr').then((signalR) => {
      // 构建SignalR连接URL
      // 在浏览器环境中使用外部可访问的后端服务地址
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
          const userId = localStorage.getItem('userId');
          if (userId) {
            this.hubConnection.invoke('JoinUserGroup', userId)
              .catch(err => console.error('加入用户组失败:', err));
          }
        })
        .catch(err => console.error('SignalR连接失败:', err));
    });
  }

  // 获取菜单
  async getMenus() {
    // 检查本地缓存是否有效
    if (this.isCacheValid()) {
      console.log('从缓存获取菜单');
      return JSON.parse(localStorage.getItem(this.cacheKey));
    }

    // 缓存无效，从后端获取
    try {
      console.log('从后端获取菜单');
      const menus = await permissionsApi.getUserMenus();
      console.log('获取菜单成功:', menus);
      this.saveToCache(menus);
      return menus;
    } catch (error) {
      console.error('获取菜单失败:', error);
      console.error('错误详情:', error.response);
      // 尝试从缓存获取，即使已过期
      const cachedMenus = localStorage.getItem(this.cacheKey);
      if (cachedMenus) {
        console.log('从过期缓存获取菜单');
        return JSON.parse(cachedMenus);
      }
      console.log('无缓存，返回空菜单');
      return [];
    }
  }

  // 检查缓存是否有效
  isCacheValid() {
    const meta = localStorage.getItem(this.metaKey);
    if (!meta) return false;

    try {
      const { expireAt } = JSON.parse(meta);
      return new Date() < new Date(expireAt);
    } catch (error) {
      console.error('解析缓存元数据失败:', error);
      return false;
    }
  }

  // 保存到缓存
  saveToCache(menus) {
    const now = new Date();
    const expireAt = new Date(now.getTime() + 30 * 60 * 1000); // 30分钟过期

    try {
      localStorage.setItem(this.cacheKey, JSON.stringify(menus));
      localStorage.setItem(this.metaKey, JSON.stringify({
        lastModified: now.toISOString(),
        expireAt: expireAt.toISOString()
      }));
    } catch (error) {
      console.error('保存菜单缓存失败:', error);
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

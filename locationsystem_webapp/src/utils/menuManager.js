// 菜单管理工具类
import { ref } from 'vue';
import * as permissionsApi from '../api/permissions';

class MenuManager {
  constructor() {
    this.userId = this.getUserId();
    this.hubConnection = null;
    this.isConnected = ref(false);
    // 后端API地址，优先从环境变量获取，否则使用相对路径
    this.backendUrl = import.meta.env.VITE_BACKEND_URL || '';
  }

  // 获取用户ID，确保一致性
  getUserId() {
    try {
      const userInfo = JSON.parse(localStorage.getItem('user_info') || '{}');
      if (userInfo && userInfo.id) {
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
        console.log('菜单已更新');
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

  // 获取菜单，直接从API加载
  async getMenus() {
    try {
      console.log('从API获取菜单');
      const menus = await permissionsApi.getUserMenus();
      console.log('获取菜单成功:', menus);

      // 确保返回的是数组
      if (Array.isArray(menus)) {
        return menus;
      } else {
        console.error('后端返回的菜单数据不是数组:', menus);
        return [];
      }
    } catch (error) {
      console.error('获取菜单失败:', error);
      console.error('错误详情:', error.response);
      return [];
    }
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

//import { createApp } from 'vue'
//import App from './App.vue'
//import router from './router'

//const app = createApp(App)
//app.use(router)
//app.mount('#app')

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import zhCn from 'element-plus/dist/locale/zh-cn.mjs'
import api from './api'
import { needRefreshToken } from './utils/tokenUtils'
import authService from './utils/authService'

const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}

// 全局注册API
app.config.globalProperties.$api = api

// 检查token是否需要刷新
const checkTokenRefresh = () => {
  // 如果正在刷新或者没有token，直接返回
  if (authService.getIsRefreshing() || !localStorage.getItem('access_token')) {
    return
  }

  if (needRefreshToken()) {
    // 主动刷新token
    authService.refreshToken().catch(error => {
      console.error('刷新token失败:', error)
      // 刷新失败，跳转到登录页
      router.push('/login')
    })
  }
}

// 应用启动时检查token
checkTokenRefresh()

// 每5分钟检查一次token
setInterval(checkTokenRefresh, 5 * 60 * 1000)

app.use(router)
app.use(ElementPlus, { locale: zhCn })
app.mount('#app')

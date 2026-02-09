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

const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}

// 全局注册API
app.config.globalProperties.$api = api

// 检查token是否需要刷新
const checkTokenRefresh = () => {
  if (needRefreshToken()) {
    const refreshToken = localStorage.getItem('refresh_token')
    const userType = localStorage.getItem('user_type')
    
    if (refreshToken && userType) {
      // 主动刷新token
      api.auth.refreshToken({
        RefreshToken: refreshToken,
        Type: parseInt(userType)
      }).then(res => {
        if (res.status === 200) {
          const authData = res.data
          localStorage.setItem('access_token', authData.accessToken)
          localStorage.setItem('refresh_token', authData.refreshToken)
        }
      }).catch(error => {
        console.error('刷新token失败:', error)
        // 刷新失败，跳转到登录页
        localStorage.removeItem('access_token')
        localStorage.removeItem('refresh_token')
        localStorage.removeItem('user_type')
        localStorage.removeItem('user_info')
        router.push('/login')
      })
    }
  }
}

// 应用启动时检查token
checkTokenRefresh()

// 每5分钟检查一次token
setInterval(checkTokenRefresh, 5 * 60 * 1000)

app.use(router)
app.use(ElementPlus, { locale: zhCn })
app.mount('#app')
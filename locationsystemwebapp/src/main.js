import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import router from './router'
import * as apiServices from './api/services'

const app = createApp(App)

// 使用 Element Plus
app.use(ElementPlus)

// 使用路由
app.use(router)

// 将 API 服务挂载到 Vue 实例上
app.config.globalProperties.$api = apiServices

app.mount('#app')

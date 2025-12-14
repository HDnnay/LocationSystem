import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import * as apiServices from './api/services'

const app = createApp(App)

// 将 API 服务挂载到 Vue 实例上
app.config.globalProperties.$api = apiServices

app.mount('#app')

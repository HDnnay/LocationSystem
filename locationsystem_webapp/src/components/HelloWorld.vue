<script setup>
import { ref, onMounted } from 'vue'
import request from '../api/request'

defineProps({
  msg: {
    type: String,
    required: true,
  },
})

// 响应式变量存储接口数据
const dentists = ref([])
const loading = ref(false)
const error = ref('')

// 调用接口的函数
const fetchDentists = async () => {
  loading.value = true
  error.value = ''
  try {
    const response = await request.get('/api/Dentists?Page=1&PageSize=10')
    // 修复：直接使用 response.data，因为响应拦截器返回的是完整的响应对象
    dentists.value = response.data.data || []
  } catch (err) {
    error.value = err.message || '请求失败'
    console.error('获取牙医列表失败:', err)
  } finally {
    loading.value = false
  }
}

// 组件挂载时调用接口
onMounted(() => {
  fetchDentists()
})
</script>

<template>
  <div class="greetings">
    <h1 class="green">{{ msg }}</h1>
    <h3>
      You’ve successfully created a project with
      <a href="https://vite.dev/" target="_blank" rel="noopener">Vite</a> +
      <a href="https://vuejs.org/" target="_blank" rel="noopener">Vue 3</a>.
    </h3>

    <!-- 接口测试部分 -->
    <div class="api-test">
      <h2>接口测试：获取牙医列表</h2>
      <div v-if="loading" class="loading">加载中...</div>
      <div v-else-if="error" class="error">{{ error }}</div>
      <div v-else-if="dentists.length > 0" class="result">
        <h3>牙医列表 (第 1 页，共 {{ dentists.length }} 条)</h3>
        <ul>
          <li v-for="dentist in dentists" :key="dentist.id">{{ dentist.name }}</li>
        </ul>
      </div>
      <div v-else class="empty">暂无数据</div>
    </div>
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  position: relative;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

/* 接口测试部分样式 */
.api-test {
  margin-top: 2rem;
  padding: 1.5rem;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  background-color: #f9f9f9;
}

.api-test h2 {
  font-size: 1.5rem;
  margin-bottom: 1rem;
  color: #333;
}

.loading {
  color: #666;
  font-style: italic;
}

.error {
  color: #ff4444;
  padding: 0.5rem;
  background-color: #ffebee;
  border-radius: 4px;
}

.result h3 {
  margin-bottom: 1rem;
  text-align: left;
}

.result ul {
  list-style-type: disc;
  padding-left: 1.5rem;
}

.result li {
  margin-bottom: 0.5rem;
  color: #555;
}

.empty {
  color: #999;
  font-style: italic;
}

@media (min-width: 1024px) {
  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style>

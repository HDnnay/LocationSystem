<template>
  <div class="dashboard-container">
    <div class="dashboard-header">
      <h2>仪表盘</h2>
    </div>

    <!-- 数据统计卡片 -->
    <div class="stats-container">
      <el-card shadow="hover" class="stat-card" v-for="stat in stats" :key="stat.id">
        <div class="stat-content">
          <div class="stat-icon" :class="stat.iconClass">{{ stat.icon }}</div>
          <div class="stat-info">
            <div class="stat-title">{{ stat.title }}</div>
            <div class="stat-value">{{ stat.value }}</div>
            <div class="stat-description">{{ stat.description }}</div>
          </div>
        </div>
      </el-card>
    </div>

    <!-- 数据概览表格 -->
    <el-card shadow="hover" class="overview-card">
      <template #header>
        <div class="card-header">
          <span>数据概览</span>
        </div>
      </template>
      <el-table :data="recentData" stripe style="width: 100%">
        <el-table-column type="index" label="序号" width="80" align="center" />
        <el-table-column prop="type" label="类型" />
        <el-table-column prop="name" label="名称" />
        <el-table-column prop="status" label="状态" />
        <el-table-column prop="date" label="更新时间" width="180" />
      </el-table>
    </el-card>

    <!-- 图表区域 -->
    <div class="charts-container">
      <!-- 饼图：数据占比 -->
      <el-card shadow="hover" class="chart-card">
        <template #header>
          <div class="card-header">
            <span>数据占比分析</span>
          </div>
        </template>
        <v-chart :option="pieOption" style="height: 350px" />
      </el-card>


    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { use } from 'echarts/core'
import { CanvasRenderer } from 'echarts/renderers'
import { PieChart, BarChart, LineChart } from 'echarts/charts'
import { TitleComponent, TooltipComponent, LegendComponent, GridComponent } from 'echarts/components'
import VChart from 'vue-echarts'

// 注册 ECharts 组件
use([
  CanvasRenderer,
  PieChart, BarChart, LineChart,
  TitleComponent, TooltipComponent, LegendComponent, GridComponent
])

// 统计数据
const stats = ref([
  { id: 1, title: '公司总数', value: 0, description: '当前系统中的公司数量', icon: '🏢', iconClass: 'icon-company' },
  { id: 2, title: '租房总数', value: 0, description: '当前系统中的租房数量', icon: '🏠', iconClass: 'icon-rent' },
  { id: 3, title: '用户总数', value: 0, description: '当前系统中的用户数量', icon: '👥', iconClass: 'icon-user' }
])

// 最近数据
const recentData = ref([
  { type: '公司', name: '示例公司', status: '活跃', date: '2024-01-15 10:30:00' },
  { type: '租房', name: '示例租房', status: '可用', date: '2024-01-15 09:15:00' },
  { type: '用户', name: '示例用户', status: '在线', date: '2024-01-14 16:45:00' }
])

// 图表配置
const pieOption = ref({
  title: {
    text: '各实体数据占比',
    left: 'center'
  },
  tooltip: {
    trigger: 'item',
    formatter: '{b}: {c} ({d}%)'
  },
  legend: {
    orient: 'vertical',
    left: 'left',
    data: ['公司', '租房', '用户']
  },
  series: [
    {
      name: '数据占比',
      type: 'pie',
      radius: '50%',
      data: [
        { value: 0, name: '公司' },
        { value: 0, name: '租房' },
        { value: 0, name: '用户' }
      ],
      emphasis: {
        itemStyle: {
          shadowBlur: 10,
          shadowOffsetX: 0,
          shadowColor: 'rgba(0, 0, 0, 0.5)'
        }
      }
    }
  ]
})

// 加载状态
const loading = ref(false)

// 获取统计数据
const fetchStats = async () => {
  loading.value = true
  try {
    // 模拟数据加载
    setTimeout(() => {
      // 更新统计数据
      stats.value[0].value = 10
      stats.value[1].value = 20
      stats.value[2].value = 30

      // 更新饼图数据
      pieOption.value.series[0].data = [
        { value: stats.value[0].value, name: '公司' },
        { value: stats.value[1].value, name: '租房' },
        { value: stats.value[2].value, name: '用户' }
      ]

      ElMessage.success('数据加载成功')
      loading.value = false
    }, 1000)
  } catch (error) {
    console.error('获取统计数据失败:', error)
    ElMessage.error('数据加载失败，请稍后重试')
    loading.value = false
  }
}

// 组件挂载时获取数据
onMounted(() => {
  fetchStats()
})
</script>

<style scoped>
.dashboard-container {
  padding: 0;
}

.dashboard-header {
  margin-bottom: 20px;
}

.dashboard-header h2 {
  margin: 0;
  font-size: 24px;
  font-weight: 600;
  color: #333;
}

.stats-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.stat-card {
  transition: transform 0.2s;
}

.stat-card:hover {
  transform: translateY(-5px);
}

.stat-content {
  display: flex;
  align-items: center;
  padding: 10px 0;
}

.stat-icon {
  font-size: 48px;
  margin-right: 20px;
  padding: 15px;
  border-radius: 50%;
  background-color: rgba(0, 0, 0, 0.05);
}

.icon-dentist {
  color: #409EFF;
}

.icon-patient {
  color: #67C23A;
}

.icon-appointment {
  color: #E6A23C;
}

.icon-office {
  color: #F56C6C;
}

.stat-info {
  flex: 1;
}

.stat-title {
  font-size: 14px;
  color: #909399;
  margin-bottom: 5px;
}

.stat-value {
  font-size: 32px;
  font-weight: bold;
  color: #303133;
  margin-bottom: 5px;
}

.stat-description {
  font-size: 12px;
  color: #909399;
}

.overview-card {
  margin-bottom: 30px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header span {
  font-size: 16px;
  font-weight: 600;
}

/* 图表区域样式 */
.charts-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 20px;
  margin-bottom: 30px;
}

.chart-card {
  transition: transform 0.2s;
}

.chart-card:hover {
  transform: translateY(-5px);
}

@media (max-width: 768px) {
  .stats-container {
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  }

  .charts-container {
    grid-template-columns: 1fr;
  }

  .stat-value {
    font-size: 24px;
  }
}
</style>

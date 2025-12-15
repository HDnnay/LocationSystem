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

      <!-- 柱状图：预约统计 -->
      <el-card shadow="hover" class="chart-card">
        <template #header>
          <div class="card-header">
            <span>预约统计</span>
          </div>
        </template>
        <v-chart :option="barOption" style="height: 350px" />
      </el-card>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getDentists } from '../api/dentists'
import { getPatients } from '../api/patients'
import { getAppointments } from '../api/appointments'
import { getDentalOffices } from '../api/dentalOffices'
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
  { id: 1, title: '牙医总数', value: 0, description: '当前系统中的牙医数量', icon: '👨‍⚕️', iconClass: 'icon-dentist' },
  { id: 2, title: '患者总数', value: 0, description: '当前系统中的患者数量', icon: '👤', iconClass: 'icon-patient' },
  { id: 3, title: '预约总数', value: 0, description: '当前系统中的预约数量', icon: '📅', iconClass: 'icon-appointment' },
  { id: 4, title: '牙科诊所', value: 0, description: '当前系统中的诊所数量', icon: '🏥', iconClass: 'icon-office' }
])

// 最近数据
const recentData = ref([
  { type: '牙医', name: '张三', status: '活跃', date: '2024-01-15 10:30:00' },
  { type: '患者', name: '李四', status: '已就诊', date: '2024-01-15 09:15:00' },
  { type: '预约', name: '王五', status: '待确认', date: '2024-01-16 14:00:00' },
  { type: '诊所', name: '阳光牙科', status: '营业中', date: '2024-01-14 16:45:00' }
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
    data: ['牙医', '患者', '预约', '牙科诊所']
  },
  series: [
    {
      name: '数据占比',
      type: 'pie',
      radius: '50%',
      data: [
        { value: 0, name: '牙医' },
        { value: 0, name: '患者' },
        { value: 0, name: '预约' },
        { value: 0, name: '牙科诊所' }
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

// 柱状图配置
const barOption = ref({
  title: {
    text: '最近7天预约统计',
    left: 'center'
  },
  tooltip: {
    trigger: 'axis',
    axisPointer: {
      type: 'shadow'
    }
  },
  legend: {
    data: ['预约数量'],
    top: 30
  },
  grid: {
    left: '3%',
    right: '4%',
    bottom: '3%',
    containLabel: true
  },
  xAxis: {
    type: 'category',
    data: []
  },
  yAxis: {
    type: 'value'
  },
  series: [
    {
      name: '预约数量',
      type: 'bar',
      data: [],
      itemStyle: {
        color: '#409EFF'
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
    // 并行获取所有统计数据
    const [dentistsRes, patientsRes, appointmentsRes, officesRes] = await Promise.all([
      getDentists({ Page: 1, PageSize: 1 }),
      getPatients({ Page: 1, PageSize: 1 }),
      getAppointments({ Page: 1, PageSize: 1 }),
      getDentalOffices({ Page: 1, PageSize: 1 })
    ])

    // 更新统计数据
    stats.value[0].value = dentistsRes.totalCount || 0
    stats.value[1].value = patientsRes.totalCount || 0
    stats.value[2].value = appointmentsRes.totalCount || 0
    stats.value[3].value = officesRes.totalCount || 0

    // 更新饼图数据
    pieOption.value.series[0].data = [
      { value: stats.value[0].value, name: '牙医' },
      { value: stats.value[1].value, name: '患者' },
      { value: stats.value[2].value, name: '预约' },
      { value: stats.value[3].value, name: '牙科诊所' }
    ]

    // 更新柱状图数据（生成最近7天的模拟数据）
    const today = new Date()
    const dates = []
    const appointmentCounts = []

    for (let i = 6; i >= 0; i--) {
      const date = new Date(today)
      date.setDate(date.getDate() - i)
      dates.push(`${date.getMonth() + 1}/${date.getDate()}`)
      // 生成随机预约数量
      appointmentCounts.push(Math.floor(Math.random() * 20) + 1)
    }

    barOption.value.xAxis.data = dates
    barOption.value.series[0].data = appointmentCounts

    ElMessage.success('数据加载成功')
  } catch (error) {
    console.error('获取统计数据失败:', error)
    ElMessage.error('数据加载失败，请稍后重试')
  } finally {
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

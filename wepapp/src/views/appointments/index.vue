<template>
  <div class="appointments">
    <!-- 页面头部区域 -->
    <div class="page-header gradient-bg">
      <div class="header-content">
        <div class="page-info">
          <h2 class="page-title">预约管理</h2>
          <p class="page-description">管理预约信息</p>
        </div>
        <div class="header-actions">
          <el-button type="primary" @click="showAddDialog">
            <el-icon><Plus /></el-icon> 新增预约
          </el-button>
        </div>
      </div>
    </div>

    <!-- 统计卡片区域 -->
    <div class="stats-grid">
      <!-- 预约总数卡片 -->
      <div class="stat-card appointment-card">
        <div class="stat-icon appointment-icon">📅</div>
        <div class="stat-content">
          <h3 class="stat-number">{{ appointments.length }}</h3>
          <p class="stat-label">预约总数</p>
        </div>
        <div class="stat-status">
          <span class="status-text">共管理{{ appointments.length }}个预约</span>
        </div>
      </div>

      <!-- 今日预约卡片 -->
      <div class="stat-card today-card">
        <div class="stat-icon today-icon">🌞</div>
        <div class="stat-content">
          <h3 class="stat-number">{{ todayAppointments }}</h3>
          <p class="stat-label">今日预约</p>
        </div>
        <div class="stat-status">
          <span class="status-text">今日{{ todayAppointments }}个预约</span>
        </div>
      </div>

      <!-- 待处理预约卡片 -->
      <div class="stat-card pending-card">
        <div class="stat-icon pending-icon">⏳</div>
        <div class="stat-content">
          <h3 class="stat-number">{{ pendingAppointments }}</h3>
          <p class="stat-label">待处理</p>
        </div>
        <div class="stat-status">
          <span class="status-text">{{ pendingAppointments }}个待处理预约</span>
        </div>
      </div>
    </div>

    <!-- 搜索和筛选区域 -->
    <div class="search-filter-section">
      <div class="search-box">
        <el-input v-model="searchKeyword"
                  placeholder="搜索患者姓名或预约ID"
                  clearable
                  @input="handleSearch">
          <template #prefix>
            <el-icon><Search /></el-icon>
          </template>
        </el-input>
      </div>
      <div class="filter-box">
        <el-select v-model="filterStatus"
                   placeholder="预约状态"
                   @change="handleFilter"
                   style="width: 160px; margin-right: 10px;">
          <el-option label="全部状态" value="" />
          <el-option label="已完成" value="completed" />
          <el-option label="待处理" value="pending" />
          <el-option label="已取消" value="cancelled" />
        </el-select>
        <el-date-picker v-model="filterDate"
                        type="date"
                        placeholder="选择预约日期"
                        style="width: 180px;"
                        @change="handleFilter" />
      </div>
    </div>

    <!-- 预约列表表格 -->
    <div class="appointment-table-section card">
      <div class="table-header">
        <h3 class="section-title">预约列表</h3>
        <div class="table-actions">
          <el-button type="info" size="small">
            <el-icon><Download /></el-icon> 导出
          </el-button>
          <el-button type="danger" size="small" @click="handleBatchDelete">
            <el-icon><Delete /></el-icon> 批量删除
          </el-button>
        </div>
      </div>

      <el-table :data="paginatedAppointments"
                style="width: 100%"
                @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" />
        <el-table-column prop="id" label="预约ID" width="120" sortable />
        <el-table-column prop="patientName" label="患者姓名" width="120" sortable />
        <el-table-column prop="dentistName" label="医生姓名" width="120" sortable />
        <el-table-column prop="dentalOffice" label="牙科诊所" width="150" />
        <el-table-column prop="appointmentDate" label="预约日期" width="130" sortable />
        <el-table-column prop="appointmentTime" label="预约时间" width="120" />
        <el-table-column prop="status" label="状态" width="120">
          <template #default="scope">
            <el-tag :type="getStatusColor(scope.row.status)">
              {{ getStatusText(scope.row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="description" label="备注" width="200" />
        <el-table-column label="操作" width="180" fixed="right">
          <template #default="scope">
            <el-button type="primary" size="small" @click="showEditDialog(scope.row)">
              <el-icon><Edit /></el-icon> 编辑
            </el-button>
            <el-button type="danger" size="small" @click="handleDelete(scope.row.id)">
              <el-icon><Delete /></el-icon> 删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <div class="pagination">
        <el-pagination @size-change="handleSizeChange"
                       @current-change="handleCurrentChange"
                       :current-page="currentPage"
                       :page-sizes="[10, 20, 50, 100]"
                       :page-size="pageSize"
                       layout="total, sizes, prev, pager, next, jumper"
                       :total="filteredAppointments.length" />
      </div>
    </div>

    <!-- 添加预约对话框 -->
    <el-dialog v-model="dialogVisible"
               :title="isEdit ? '编辑预约' : '新增预约'"
               width="500px">
      <el-form :model="formData" label-width="80px">
        <el-form-item label="患者姓名">
          <el-input v-model="formData.patientName" placeholder="请输入患者姓名" />
        </el-form-item>
        <el-form-item label="医生姓名">
          <el-select v-model="formData.dentistName" placeholder="请选择医生">
            <el-option v-for="dentist in dentists" :key="dentist" :label="dentist" :value="dentist" />
          </el-select>
        </el-form-item>
        <el-form-item label="牙科诊所">
          <el-select v-model="formData.dentalOffice" placeholder="请选择牙科诊所">
            <el-option v-for="office in dentalOffices" :key="office" :label="office" :value="office" />
          </el-select>
        </el-form-item>
        <el-form-item label="预约日期">
          <el-date-picker v-model="formData.appointmentDate"
                          type="date"
                          placeholder="选择预约日期"
                          style="width: 100%" />
        </el-form-item>
        <el-form-item label="预约时间">
          <el-time-picker v-model="formData.appointmentTime"
                          placeholder="选择预约时间"
                          style="width: 100%" />
        </el-form-item>
        <el-form-item label="状态">
          <el-select v-model="formData.status" placeholder="请选择状态">
            <el-option label="已完成" value="completed" />
            <el-option label="待处理" value="pending" />
            <el-option label="已取消" value="cancelled" />
          </el-select>
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="formData.description" type="textarea" rows="2" placeholder="请输入备注" />
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSave">保存</el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script>
  import { Plus, Search, Download, Edit, Delete } from '@element-plus/icons-vue'
  import api from '@/api'

  export default {
    name: 'Appointments',
    components: {
      Plus,
      Search,
      Download,
      Edit,
      Delete
    },
    data() {
      return {
        // 页面数据
        currentDate: '',
        // 搜索筛选
        searchKeyword: '',
        filterStatus: '',
        filterDate: '',
        // Dentist list (for dropdown)
        dentists: [],
        // Dental office list (for dropdown)
        dentalOffices: [],
        // 预约列表
        appointments: [],
        // 分页
        currentPage: 1,
        pageSize: 10,
        // 选中项
        selectedAppointments: [],
        // 对话框
        dialogVisible: false,
        isEdit: false,
        formData: {
          id: '',
          patientName: '',
          dentistName: '',
          dentalOffice: '',
          appointmentDate: '',
          appointmentTime: '',
          status: 'pending',
          description: ''
        }
      }
    },
    computed: {
      // 过滤后的预约列表
      filteredAppointments() {
        let result = [...this.appointments]

        // 关键词搜索
        if (this.searchKeyword) {
          const keyword = this.searchKeyword.toLowerCase()
          result = result.filter(item =>
            item.patientName.toLowerCase().includes(keyword) ||
            item.dentistName.toLowerCase().includes(keyword) ||
            item.id.toLowerCase().includes(keyword) ||
            item.dentalOffice.toLowerCase().includes(keyword)
          )
        }

        // 状态筛选
        if (this.filterStatus) {
          result = result.filter(item => item.status === this.filterStatus)
        }

        // 日期筛选
        if (this.filterDate) {
          result = result.filter(item => item.appointmentDate === this.filterDate)
        }

        return result
      },
      // 分页后的预约列表
      paginatedAppointments() {
        const startIndex = (this.currentPage - 1) * this.pageSize
        const endIndex = startIndex + this.pageSize
        return this.filteredAppointments.slice(startIndex, endIndex)
      },
      // 统计数据
      todayAppointments() {
        // 计算今日预约数
        const today = this.currentDate
        return this.appointments.filter(item => item.appointmentDate === today).length
      },
      pendingAppointments() {
        return this.appointments.filter(item => item.status === 'pending').length
      }
    },
    created() {
      // 初始化当前日期
      const date = new Date()
      this.currentDate = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`
    },
    mounted() {
      this.getAppointments({ page: this.currentPage, pageSize: this.pageSize })
      this.getDentists()
      this.getDentalOffices()
    },
    methods: {
      // 获取牙医列表
      async getDentists() {
        try {
          const parms = {
            page: this.currentPage,
            pageSize: 10
          }
          const res = await api.dentists.getDentists(parms)
          this.dentists = res.data.map(dentist => dentist.name)
        } catch (error) {
          this.$message.error('获取牙医列表失败')
          console.error('获取牙医列表失败:', error)
        }
      },
      // 获取牙科诊所列表
      async getDentalOffices() {
        try {
          const parms = {
            page: this.currentPage,
            pageSize:10
          }
          const res = await api.dentalOffices.getDentalOffices(parms)
          this.dentalOffices = res.data.map(office => office.name)
        } catch (error) {
          this.$message.error('获取牙科诊所列表失败')
          console.error('获取牙科诊所列表失败:', error)
        }
      },
      // 获取预约列表
      async getAppointments() {
        try {
          const parms = {
            page: this.currentPage,
            pageSize: 10
          }
          const res = await api.appointments.getAppointments(parms)
          this.appointments = res.data
        } catch (error) {
          this.$message.error('获取预约列表失败')
          console.error('获取预约列表失败:', error)
        }
      },
      // 获取状态文本
      getStatusText(status) {
        const statusMap = {
          completed: '已完成',
          pending: '待处理',
          cancelled: '已取消'
        }
        return statusMap[status] || status
      },
      // 获取状态颜色
      getStatusColor(status) {
        const colorMap = {
          completed: 'success',
          pending: 'warning',
          cancelled: 'danger'
        }
        return colorMap[status] || 'info'
      },
      // 搜索
      handleSearch() {
        this.currentPage = 1 // 搜索时重置到第一页
      },
      // 筛选
      handleFilter() {
        this.currentPage = 1 // 筛选时重置到第一页
      },
      // 显示添加对话框
      showAddDialog() {
        this.isEdit = false
        this.formData = {
          id: '',
          patientName: '',
          dentistName: '',
          dentalOffice: '',
          appointmentDate: '',
          appointmentTime: '',
          status: 'pending',
          description: ''
        }
        this.dialogVisible = true
      },
      // 显示编辑对话框
      showEditDialog(row) {
        this.isEdit = true
        this.formData = { ...row }
        this.dialogVisible = true
      },
      // 保存预约
      async handleSave() {
        try {
          if (this.isEdit) {
            // 编辑现有预约
            await api.appointments.updateAppointment(this.formData.id, this.formData)
            this.$message.success('预约更新成功')
          } else {
            // 添加新预约
            await api.appointments.createAppointment(this.formData)
            this.$message.success('预约添加成功')
          }
          this.dialogVisible = false
          this.getAppointments() // 重新获取预约列表
        } catch (error) {
          this.$message.error(this.isEdit ? '预约更新失败' : '预约添加失败')
          console.error(this.isEdit ? '预约更新失败:' : '预约添加失败:', error)
        }
      },
      // 删除预约
      async handleDelete(id) {
        this.$confirm('确定要删除这个预约吗？', '删除确认', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(async () => {
          try {
            await api.appointments.deleteAppointment(id)
            this.$message.success('预约删除成功')
            this.getAppointments() // 重新获取预约列表
          } catch (error) {
            this.$message.error('预约删除失败')
            console.error('预约删除失败:', error)
          }
        }).catch(() => {
          this.$message.info('已取消删除')
        })
      },
      // 批量删除
      async handleBatchDelete() {
        if (this.selectedAppointments.length === 0) {
          this.$message.warning('请选择要删除的预约')
          return
        }

        this.$confirm(`确定要删除选中的${this.selectedAppointments.length}个预约吗？`, '删除确认', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(async () => {
          try {
            const selectedIds = this.selectedAppointments.map(item => item.id)
            for (const id of selectedIds) {
              await api.appointments.deleteAppointment(id)
            }
            this.$message.success('预约删除成功')
            this.selectedAppointments = []
            this.getAppointments() // 重新获取预约列表
          } catch (error) {
            this.$message.error('预约删除失败')
            console.error('预约删除失败:', error)
          }
        }).catch(() => {
          this.$message.info('已取消删除')
        })
      },
      // 处理选择变化
      handleSelectionChange(val) {
        this.selectedAppointments = val
      },
      // 分页大小变化
      handleSizeChange(val) {
        this.pageSize = val
        this.currentPage = 1
      },
      // 当前页码变化
      handleCurrentChange(val) {
        this.currentPage = val
      }
    }
  }
</script>

<style scoped>
  .appointments {
    min-height: 100vh;
    background-color: #f5f7fa;
  }

  /* 页面头部 */
  .page-header {
    padding: 20px 30px;
    margin-bottom: 20px;
  }

  .header-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    max-width: 1400px;
    margin: 0 auto;
  }

  .page-title {
    margin: 0 0 5px 0;
    font-size: 28px;
    font-weight: 600;
    color: #fff;
  }

  .page-description {
    margin: 0;
    color: rgba(255, 255, 255, 0.8);
    font-size: 14px;
  }

  /* 统计卡片 */
  .stats-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 20px;
    padding: 0 30px;
    margin-bottom: 20px;
  }

  .stat-card {
    background-color: #fff;
    border-radius: 8px;
    padding: 20px;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    display: flex;
    align-items: center;
    transition: transform 0.3s ease;
  }

    .stat-card:hover {
      transform: translateY(-3px);
    }

  .stat-icon {
    font-size: 40px;
    margin-right: 20px;
    width: 60px;
    height: 60px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 8px;
  }

  .appointment-icon {
    background-color: #e6f7ff;
    color: #1890ff;
  }

  .today-icon {
    background-color: #f6ffed;
    color: #52c41a;
  }

  .pending-icon {
    background-color: #fffbe6;
    color: #faad14;
  }

  .stat-content {
    flex: 1;
  }

  .stat-number {
    margin: 0;
    font-size: 28px;
    font-weight: 600;
    color: #333;
  }

  .stat-label {
    margin: 5px 0 0 0;
    color: #999;
    font-size: 14px;
  }

  .stat-status {
    text-align: right;
  }

  .status-text {
    color: #999;
    font-size: 12px;
  }

  /* 搜索筛选区域 */
  .search-filter-section {
    background-color: #fff;
    padding: 20px 30px;
    margin-bottom: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    gap: 15px;
  }

  .search-box {
    flex: 1;
    min-width: 300px;
  }

  .filter-box {
    display: flex;
    gap: 10px;
  }

  /* 表格区域 */
  .appointment-table-section {
    background-color: #fff;
    padding: 20px;
    margin: 0 30px 30px 30px;
    border-radius: 8px;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  }

  .table-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

  .section-title {
    margin: 0;
    font-size: 18px;
    font-weight: 600;
    color: #333;
  }

  .table-actions {
    display: flex;
    gap: 10px;
  }

  /* 分页 */
  .pagination {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
  }

  /* 响应式设计 */
  @media (max-width: 768px) {
    .header-content {
      flex-direction: column;
      align-items: flex-start;
      gap: 15px;
    }

    .stats-grid {
      grid-template-columns: 1fr;
    }

    .search-filter-section {
      flex-direction: column;
      align-items: stretch;
    }

    .table-header {
      flex-direction: column;
      align-items: flex-start;
      gap: 15px;
    }

    .table-actions {
      width: 100%;
      justify-content: flex-start;
    }
  }
</style>

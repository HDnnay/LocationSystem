

<template>
  <div class="appointments-container">
    <!-- 页面头部 -->
    <div class="page-header" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
      <div>
        <h2 style="margin: 0; font-size: 20px; font-weight: 600;">预约管理</h2>
        <p style="margin: 5px 0 0; color: #606266; font-size: 14px;">管理系统中的预约信息，包括牙医、患者和时间安排</p>
      </div>
      <el-button type="primary" @click="openAddDialog" :icon="Plus">新增预约</el-button>
    </div>

    <!-- 搜索栏 -->
    <div class="search-bar" style="background-color: #fff; padding: 20px; border-radius: 4px; box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1); margin-bottom: 20px;">
      <el-row :gutter="20" align="middle">
        <el-col :span="6">
          <el-input
            v-model="queryParams.keyword"
            placeholder="请输入关键词搜索"
            :prefix-icon="Search"
            style="width: 100%;"
          >
          </el-input>
        </el-col>
        <el-col :span="6">
          <el-button type="primary" @click="searchAppointments" style="margin-right: 10px;">搜索</el-button>
          <el-button @click="resetSearch">重置</el-button>
        </el-col>
      </el-row>
    </div>

    <!-- 预约列表 -->
    <div style="background-color: #fff; border-radius: 4px; box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1); overflow: hidden;">
      <el-table
        v-loading="loading"
        :data="appointments"
        border
        style="width: 100%; border: none;"
      >
      <el-table-column type="index" label="序号" width="80" />
      <el-table-column label="牙医" width="120">
        <template #default="scope">
          {{ scope.row.dentist?.name || dentists.find(d => d.id === scope.row.dentistId)?.name }}
        </template>
      </el-table-column>
      <el-table-column label="患者" width="120">
        <template #default="scope">
          {{ scope.row.patient?.name || patients.find(p => p.id === scope.row.patientId)?.name }}
        </template>
      </el-table-column>
      <el-table-column label="牙科诊所" width="150">
        <template #default="scope">
          {{ scope.row.dentalOffice?.name }}
        </template>
      </el-table-column>
      <el-table-column prop="startTime" label="开始时间" width="180" />
      <el-table-column prop="endTime" label="结束时间" width="180" />
      <el-table-column prop="status" label="状态" width="100">
        <template #default="scope">
          <el-tag
            :type="scope.row.status === 'Completed' ? 'success' : scope.row.status === 'Cancelled' ? 'danger' : 'warning'"
          >
            {{ scope.row.status }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="150" fixed="right">
        <template #default="scope">
          <el-button
            type="primary"
            size="small"
            @click="openEditDialog(scope.row)"
            :icon="Edit"
          >编辑</el-button>
          <el-button
            type="danger"
            size="small"
            @click="deleteAppointment(scope.row.id)"
            :icon="Delete"
          >删除</el-button>
        </template>
      </el-table-column>
      </el-table>

      <!-- 分页组件 -->
      <div style="padding: 15px; display: flex; justify-content: flex-end;">
        <el-pagination
          v-model:current-page="currentPage"
          v-model:page-size="pageSize"
          :page-sizes="[5, 10, 20, 50]"
          layout="total, sizes, prev, pager, next, jumper"
          :total="total"
          @size-change="handleSizeChange"
          @current-change="handlePageChange"
        />
      </div>
    </div>

    <!-- 表单对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="isEditMode ? '编辑预约' : '新增预约'"
      width="500px"
    >
      <el-form
        ref="appointmentForm"
        :model="currentAppointment"
        :rules="rules"
        label-width="100px"
      >
        <el-form-item label="牙医" prop="dentistId">
          <el-select
            v-model="currentAppointment.dentistId"
            placeholder="请选择牙医"
          >
            <el-option
              v-for="dentist in dentists"
              :key="dentist.id"
              :label="dentist.name"
              :value="dentist.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="患者" prop="patientId">
          <el-select
            v-model="currentAppointment.patientId"
            placeholder="请选择患者"
          >
            <el-option
              v-for="patient in patients"
              :key="patient.id"
              :label="patient.name"
              :value="patient.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="牙科诊所" prop="dentalOfficeId">
          <el-select
            v-model="currentAppointment.dentalOfficeId"
            placeholder="请选择牙科诊所"
          >
            <el-option
              v-for="office in dentalOffices"
              :key="office.id"
              :label="office.name"
              :value="office.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="开始时间" prop="startTime">
          <el-date-picker
            v-model="currentAppointment.startTime"
            type="datetime"
            placeholder="选择开始时间"
            style="width: 100%"
          />
        </el-form-item>
        <el-form-item label="结束时间" prop="endTime">
          <el-date-picker
            v-model="currentAppointment.endTime"
            type="datetime"
            placeholder="选择结束时间"
            style="width: 100%"
          />
        </el-form-item>
        <el-form-item label="状态">
          <el-select
            v-model="currentAppointment.status"
            placeholder="请选择状态"
          >
            <el-option label="Pending" value="Pending" />
            <el-option label="Completed" value="Completed" />
            <el-option label="Cancelled" value="Cancelled" />
          </el-select>
        </el-form-item>

      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="saveAppointment">确定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>
<script setup>
  import { ref, onMounted } from 'vue'
  import { ElMessage, ElMessageBox } from 'element-plus'
  import { Plus, Edit, Delete, Search } from '@element-plus/icons-vue'
  import appointmentService from '@/api/services/appointmentService'
  import dentistService from '@/api/services/dentistService'
  import patientService from '@/api/services/patientService'
  import dentalOfficeService from '@/api/services/dentalOfficeService'

  // 预约列表数据
  const appointments = ref([])
  // 牙医列表数据
  const dentists = ref([])
  // 患者列表数据
  const patients = ref([])
  // 牙科诊所列表数据
  const dentalOffices = ref([])
  // 加载状态
  const loading = ref(false)
  // 表单对话框显示状态
  const dialogVisible = ref(false)
  // 编辑模式
  const isEditMode = ref(false)
  // 当前编辑的预约
  const currentAppointment = ref({})
  // 查询参数
  const queryParams = ref({
    keyword: ''
  })
  // 分页参数
  const currentPage = ref(1)
  const pageSize = ref(10)
  const total = ref(0)

  // 表单验证规则
  const rules = {
    dentistId: [
      { required: true, message: '请选择牙医', trigger: 'change' }
    ],
    patientId: [
      { required: true, message: '请选择患者', trigger: 'change' }
    ],
    dentalOfficeId: [
      { required: true, message: '请选择牙科诊所', trigger: 'change' }
    ],
    startTime: [
      { required: true, message: '请选择开始时间', trigger: 'change' }
    ],
    endTime: [
      { required: true, message: '请选择结束时间', trigger: 'change' }
    ]
  }

  // 获取预约列表
  const getAppointments = async () => {
    loading.value = true
    try {
      const params = {
        ...queryParams.value,
        page: currentPage.value,
        pageSize: pageSize.value
      }
      const response = await appointmentService.getAll(params)
      appointments.value = response.data?.items || response.data || []
      total.value = response.data?.total || 0
    } catch (error) {
      ElMessage.error('获取预约列表失败')
      console.error('获取预约列表失败:', error)
    } finally {
      loading.value = false
    }
  }

  // 获取牙医列表
  const getDentists = async () => {
    try {
      const response = await dentistService.getAll()
      dentists.value = response.data || []
    } catch (error) {
      console.error('获取牙医列表失败:', error)
    }
  }

  // 获取患者列表
  const getPatients = async () => {
    try {
      const response = await patientService.getAll()
      patients.value = response.data || []
    } catch (error) {
      console.error('获取患者列表失败:', error)
    }
  }

  // 获取牙科诊所列表
  const getDentalOffices = async () => {
    try {
      const response = await dentalOfficeService.getAll()
      dentalOffices.value = response.data || []
    } catch (error) {
      console.error('获取牙科诊所列表失败:', error)
    }
  }

  // 搜索预约
  const searchAppointments = () => {
    currentPage.value = 1
    getAppointments()
  }

  // 重置搜索
  const resetSearch = () => {
    queryParams.value = {
      keyword: ''
    }
    currentPage.value = 1
    getAppointments()
  }

  // 分页变化处理
  const handlePageChange = (page) => {
    currentPage.value = page
    getAppointments()
  }

  // 每页条数变化处理
  const handleSizeChange = (size) => {
    pageSize.value = size
    currentPage.value = 1
    getAppointments()
  }

  // 打开新增对话框
  const openAddDialog = () => {
    isEditMode.value = false
    currentAppointment.value = {}
    dialogVisible.value = true
  }

  // 打开编辑对话框
  const openEditDialog = (appointment) => {
    isEditMode.value = true
    currentAppointment.value = { ...appointment }
    dialogVisible.value = true
  }

  // 保存预约信息
  const saveAppointment = async () => {
    try {
      if (isEditMode.value) {
        // 更新预约
        await appointmentService.update(currentAppointment.value.id, currentAppointment.value)
        ElMessage.success('更新成功')
      } else {
        // 新增预约
        await appointmentService.create(currentAppointment.value)
        ElMessage.success('新增成功')
      }
      dialogVisible.value = false
      getAppointments()
    } catch (error) {
      ElMessage.error(isEditMode.value ? '更新失败' : '新增失败')
      console.error('保存预约失败:', error)
    }
  }

  // 删除预约
  const deleteAppointment = (id) => {
    ElMessageBox.confirm('确定要删除该预约吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    }).then(async () => {
      try {
        await appointmentService.delete(id)
        ElMessage.success('删除成功')
        getAppointments()
      } catch (error) {
        ElMessage.error('删除失败')
        console.error('删除预约失败:', error)
      }
    }).catch(() => {
      ElMessage.info('已取消删除')
    })
  }

  // 组件挂载时获取数据
  onMounted(async () => {
    await Promise.all([
      getAppointments(),
      getDentists(),
      getPatients(),
      getDentalOffices()
    ])
  })
</script>
<style scoped>
.appointments-container {
  padding: 20px;
}

.page-header {
  margin-bottom: 20px;
}
</style>

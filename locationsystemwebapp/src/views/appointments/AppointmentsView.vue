
<script setup>
  import { ref, onMounted } from 'vue'
  import { ElMessage, ElMessageBox } from 'element-plus'
  import { Plus, Search } from '@element-plus/icons-vue'
  import appointmentService from '@/api/services/appointmentService'
  import patientService from '@/api/services/patientService'
  import dentistService from '@/api/services/dentistService'
  import dentalOfficeService from '@/api/services/dentalOfficeService'

  // 预约列表数据
  const appointments = ref([])
  // 患者列表
  const patients = ref([])
  // 牙医列表
  const dentists = ref([])
  // 诊所列表
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
    keyWord: ''
  })
  // 分页参数
  const currentPage = ref(1)
  const pageSize = ref(10)
  const total = ref(0)
  // 表单引用
  const formRef = ref(null)

  // 表单验证规则
  const rules = {
    patientId: [
      { required: true, message: '请选择患者', trigger: 'blur' }
    ],
    dentistId: [
      { required: true, message: '请选择牙医', trigger: 'blur' }
    ],
    dentalOfficeId: [
      { required: true, message: '请选择诊所', trigger: 'blur' }
    ],
    startTime: [
      { required: true, message: '请选择开始时间', trigger: 'change' }
    ],
    endTime: [
      { required: true, message: '请选择结束时间', trigger: 'change' }
    ],
    status: [
      { required: true, message: '请选择状态', trigger: 'blur' }
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
      console.log('准备调用预约列表接口，参数:', params)
      const response = await appointmentService.getAll(params)
      console.log('预约列表响应:', response)
      // 尝试多种可能的响应格式
      appointments.value = response?.Data || response?.data || []
      total.value = response?.Total || response?.total || 0
    } catch (error) {
      ElMessage.error('获取预约列表失败')
      console.error('获取预约列表失败:', error)
    } finally {
      loading.value = false
    }
  }

  // 获取患者列表
  const getPatients = async () => {
    try {
      const response = await patientService.getAll()
      patients.value = response?.Data || response?.data || []
    } catch (error) {
      console.error('获取患者列表失败:', error)
    }
  }

  // 获取牙医列表
  const getDentists = async () => {
    try {
      const response = await dentistService.getAll()
      dentists.value = response?.Data || response?.data || []
    } catch (error) {
      console.error('获取牙医列表失败:', error)
    }
  }

  // 获取诊所列表
  const getDentalOffices = async () => {
    try {
      const response = await dentalOfficeService.getAll()
      dentalOffices.value = response?.Data || response?.data || []
    } catch (error) {
      console.error('获取诊所列表失败:', error)
    }
  }

  // 搜索
  const handleSearch = () => {
    currentPage.value = 1
    getAppointments()
  }

  // 处理分页大小变化
  const handleSizeChange = (size) => {
    pageSize.value = size
    getAppointments()
  }

  // 处理当前页变化
  const handleCurrentChange = (current) => {
    currentPage.value = current
    getAppointments()
  }

  // 处理新增
  const handleAdd = () => {
    isEditMode.value = false
    currentAppointment.value = {
      status: 'Scheduled'
    }
    dialogVisible.value = true
  }

  // 处理编辑
  const handleEdit = (row) => {
    isEditMode.value = true
    currentAppointment.value = { ...row }
    dialogVisible.value = true
  }

  // 处理提交
  const handleSubmit = async () => {
    if (!formRef.value) return
    try {
      await formRef.value.validate()
      if (isEditMode.value) {
        await appointmentService.update(currentAppointment.value.id, currentAppointment.value)
        ElMessage.success('更新预约成功')
      } else {
        await appointmentService.create(currentAppointment.value)
        ElMessage.success('创建预约成功')
      }
      dialogVisible.value = false
      getAppointments()
    } catch (error) {
      console.error('提交预约失败:', error)
      ElMessage.error('操作失败，请稍后重试')
    }
  }

  // 处理删除
  const handleDelete = (id) => {
    ElMessageBox.confirm('确定要删除这个预约吗？', '提示', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    })
      .then(async () => {
        await appointmentService.delete(id)
        ElMessage.success('删除预约成功')
        getAppointments()
      })
      .catch(() => {
        // 用户取消删除
      })
  }

  // 初始化数据
  onMounted(() => {
    getAppointments()
    getPatients()
    getDentists()
    getDentalOffices()
  })
</script>
<template>
  <div class="appointments-container">
    <div class="header">
      <h2>预约管理</h2>
      <div class="header-actions">
        <el-button type="primary" :icon="Plus" @click="handleAdd">
          新增预约
        </el-button>
      </div>
    </div>

    <div class="search-container">
      <el-form :model="queryParams" :inline="true" size="small">
        <el-form-item label="关键词">
          <el-input
            v-model="queryParams.keyWord"
            placeholder="请输入患者姓名、牙医姓名或诊所名称"
            clearable
            style="width: 300px"
          >
            <template #append>
              <el-button :icon="Search" @click="handleSearch" />
            </template>
          </el-input>
        </el-form-item>
      </el-form>
    </div>

    <div class="table-container">
      <el-table
        v-loading="loading"
        :data="appointments"
        style="width: 100%"
        stripe
        border
      >
        <el-table-column prop="id" label="预约ID" width="120" />
        <el-table-column prop="patientName" label="患者" width="120" />
        <el-table-column prop="dentistName" label="牙医" width="120" />
        <el-table-column prop="dentalOfficeName" label="诊所" width="150" />
        <el-table-column prop="startTime" label="开始时间" min-width="180"
          :formatter="(row) => new Date(row.startTime).toLocaleString('zh-CN')" />
        <el-table-column prop="endTime" label="结束时间" min-width="180"
          :formatter="(row) => new Date(row.endTime).toLocaleString('zh-CN')" />
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
            <el-button size="small" @click="handleEdit(scope.row)">
              编辑
            </el-button>
            <el-button size="small" type="danger" @click="handleDelete(scope.row.id)">
              删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <div class="pagination-container">
      <el-pagination
        v-model:current-page="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
      />
    </div>

    <el-dialog
      v-model="dialogVisible"
      :title="isEditMode ? '编辑预约' : '新增预约'"
      width="500px"
    >
      <el-form :model="currentAppointment" :rules="rules" ref="formRef" label-width="100px">
        <el-form-item label="患者" prop="patientId">
          <el-select v-model="currentAppointment.patientId" placeholder="请选择患者">
            <el-option
              v-for="patient in patients"
              :key="patient.id"
              :label="patient.name"
              :value="patient.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="牙医" prop="dentistId">
          <el-select v-model="currentAppointment.dentistId" placeholder="请选择牙医">
            <el-option
              v-for="dentist in dentists"
              :key="dentist.id"
              :label="dentist.name"
              :value="dentist.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="诊所" prop="dentalOfficeId">
          <el-select v-model="currentAppointment.dentalOfficeId" placeholder="请选择诊所">
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
        <el-form-item label="状态" prop="status">
          <el-select v-model="currentAppointment.status" placeholder="请选择状态">
            <el-option label="Scheduled" value="Scheduled" />
            <el-option label="Completed" value="Completed" />
            <el-option label="Cancelled" value="Cancelled" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSubmit">确定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>



<style scoped>
.appointments-container {
  padding: 20px;
  background-color: #fff;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.header h2 {
  margin: 0;
  color: #333;
}

.search-container {
  margin-bottom: 20px;
}

.table-container {
  margin-bottom: 20px;
}

.pagination-container {
  display: flex;
  justify-content: flex-end;
}

.dialog-footer {
  text-align: right;
}
</style>

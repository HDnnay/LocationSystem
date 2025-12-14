<script setup>
import { ref, onMounted } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import { Plus, Edit, Delete, Search } from '@element-plus/icons-vue'
import patientService from '@/api/services/patientService'

// 患者列表数据
const patients = ref([])
// 加载状态
const loading = ref(false)
// 表单对话框显示状态
const dialogVisible = ref(false)
// 编辑模式
const isEditMode = ref(false)
// 当前编辑的患者
const currentPatient = ref({})
// 查询参数
const queryParams = ref({
  name: ''
})
// 分页参数
const currentPage = ref(1)
const pageSize = ref(10)
const total = ref(0)

// 表单验证规则
const rules = {
  name: [
    { required: true, message: '请输入患者姓名', trigger: 'blur' }
  ],
  phone: [
    { required: true, message: '请输入电话号码', trigger: 'blur' },
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码', trigger: 'blur' }
  ],
  email: [
    { required: true, message: '请输入邮箱', trigger: 'blur' },
    { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
  ]
}

// 获取患者列表
const getPatients = async () => {
  loading.value = true
  try {
    const params = {
      ...queryParams.value,
      page: currentPage.value,
      pageSize: pageSize.value
    }
    const response = await patientService.getAll(params)
    patients.value = response.data?.items || response.data || []
    total.value = response.data?.total || 0
  } catch (error) {
    ElMessage.error('获取患者列表失败')
    console.error('获取患者列表失败:', error)
  } finally {
    loading.value = false
  }
}

// 搜索患者
const searchPatients = () => {
  currentPage.value = 1
  getPatients()
}

// 重置搜索
const resetSearch = () => {
  queryParams.value = {
    name: ''
  }
  currentPage.value = 1
  getPatients()
}

// 分页变化处理
const handlePageChange = (page) => {
  currentPage.value = page
  getPatients()
}

// 每页条数变化处理
const handleSizeChange = (size) => {
  pageSize.value = size
  currentPage.value = 1
  getPatients()
}

// 打开新增对话框
const openAddDialog = () => {
  isEditMode.value = false
  currentPatient.value = {}
  dialogVisible.value = true
}

// 打开编辑对话框
const openEditDialog = (patient) => {
  isEditMode.value = true
  currentPatient.value = { ...patient }
  dialogVisible.value = true
}

// 保存患者信息
const savePatient = async () => {
  try {
    if (isEditMode.value) {
      // 更新患者
      await patientService.update(currentPatient.value.id, currentPatient.value)
      ElMessage.success('更新成功')
    } else {
      // 新增患者
      await patientService.create(currentPatient.value)
      ElMessage.success('新增成功')
    }
    dialogVisible.value = false
    getPatients()
  } catch (error) {
    ElMessage.error(isEditMode.value ? '更新失败' : '新增失败')
    console.error('保存患者失败:', error)
  }
}

// 删除患者
const deletePatient = (id) => {
  ElMessageBox.confirm('确定要删除该患者吗？', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning'
  }).then(async () => {
    try {
      await patientService.delete(id)
      ElMessage.success('删除成功')
      getPatients()
    } catch (error) {
      ElMessage.error('删除失败')
      console.error('删除患者失败:', error)
    }
  }).catch(() => {
    ElMessage.info('已取消删除')
  })
}

// 组件挂载时获取患者列表
onMounted(() => {
  getPatients()
})
</script>

<template>
  <div class="patients-container">
    <!-- 页面头部 -->
    <div class="page-header" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 20px;">
      <div>
        <h2 style="margin: 0; font-size: 20px; font-weight: 600;">患者管理</h2>
        <p style="margin: 5px 0 0; color: #606266; font-size: 14px;">管理系统中的患者信息，包括基本资料和联系方式</p>
      </div>
      <el-button type="primary" @click="openAddDialog" :icon="Plus">新增患者</el-button>
    </div>

    <!-- 搜索栏 -->
    <div class="search-bar" style="background-color: #fff; padding: 20px; border-radius: 4px; box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1); margin-bottom: 20px;">
      <el-row :gutter="20" align="middle">
        <el-col :span="6">
          <el-input
            v-model="queryParams.name"
            placeholder="请输入患者姓名"
            :prefix-icon="Search"
            style="width: 100%;"
          >
          </el-input>
        </el-col>
        <el-col :span="6">
          <el-button type="primary" @click="searchPatients" style="margin-right: 10px;">搜索</el-button>
          <el-button @click="resetSearch">重置</el-button>
        </el-col>
      </el-row>
    </div>

    <!-- 患者列表 -->
    <div style="background-color: #fff; border-radius: 4px; box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1); overflow: hidden;">
      <el-table
        v-loading="loading"
        :data="patients"
        border
        style="width: 100%; border: none;"
      >
      <el-table-column type="index" label="序号" width="80" />
      <el-table-column prop="name" label="姓名" width="120" />
      <el-table-column prop="email" label="邮箱" width="200" />
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
            @click="deletePatient(scope.row.id)"
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
      :title="isEditMode ? '编辑患者' : '新增患者'"
      width="500px"
    >
      <el-form
        ref="patientForm"
        :model="currentPatient"
        :rules="rules"
        label-width="100px"
      >
        <el-form-item label="姓名" prop="name">
          <el-input v-model="currentPatient.name" placeholder="请输入患者姓名" />
        </el-form-item>
        <el-form-item label="电话号码" prop="phone">
          <el-input v-model="currentPatient.phone" placeholder="请输入电话号码" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="currentPatient.email" placeholder="请输入邮箱" />
        </el-form-item>
        <el-form-item label="地址">
          <el-input v-model="currentPatient.address" placeholder="请输入地址" />
        </el-form-item>
        <el-form-item label="出生日期">
          <el-date-picker
            v-model="currentPatient.dateOfBirth"
            type="date"
            placeholder="选择出生日期"
            style="width: 100%"
          />
        </el-form-item>
        <el-form-item label="备注">
          <el-input
            v-model="currentPatient.remark"
            type="textarea"
            :rows="3"
            placeholder="请输入备注信息"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="savePatient">确定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<style scoped>
</style>

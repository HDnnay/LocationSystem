<template>
    <div class="appointments-page">
        <el-page-header>
            <template #title>
                <h1>预约管理</h1>
            </template>
        </el-page-header>

        <!-- 工具栏 -->
        <el-card class="toolbar-card" shadow="hover">
            <div class="toolbar">
                <el-input v-model="searchQuery"
                          placeholder="搜索患者ID、牙医ID或诊所ID"
                          clearable
                          style="width: 300px; margin-right: 10px;"
                          @keyup.enter="onSearch">
                    <template #append>
                        <el-button icon="el-icon-Search" @click="onSearch" :loading="loading">搜索</el-button>
                    </template>
                </el-input>
                <el-button type="primary" icon="el-icon-Plus" @click="addAppointment" :loading="loading">添加预约</el-button>
            </div>
        </el-card>

        <!-- 预约列表 -->
        <el-card class="table-card" shadow="hover">
            <el-table v-loading="loading"
                      :data="appointments"
                      style="width: 100%"
                      stripe
                      border>
                <el-table-column type="index"
                                 :index="(index) => (currentPage - 1) * pageSize + index + 1"
                                 label="序号"
                                 width="80" />
                <el-table-column prop="patient.name" label="患者姓名" min-width="120" />
                <el-table-column prop="dentist.name" label="牙医姓名" min-width="120" />
                <el-table-column prop="dentalOffice.name" label="诊所" min-width="120" />
                <el-table-column prop="startTime" label="开始时间" min-width="160">
                  <template #default="scope">
                    {{ formatDateTime(scope.row.startTime) }}
                  </template>
                </el-table-column>
                <el-table-column prop="endTime" label="结束时间" min-width="160">
                  <template #default="scope">
                    {{ formatDateTime(scope.row.endTime) }}
                  </template>
                </el-table-column>
                <el-table-column prop="status" label="状态" min-width="100">
                    <template #default="scope">
                        <el-tag :type="getStatusType(scope.row.status)">
                            {{ getStatusText(scope.row.status) }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="操作" min-width="100" fixed="right">
                    <template #default="scope">
                        <el-dropdown>
                            <el-button type="primary" size="small">
                                操作 <el-icon class="el-icon--right"><ArrowDown /></el-icon>
                            </el-button>
                            <template #dropdown>
                                <el-dropdown-menu>
                                    <el-dropdown-item>
                                        <el-button type="primary" icon="el-icon-Edit" size="small" @click.stop="editAppointment(scope.row)" :loading="loading">编辑</el-button>
                                    </el-dropdown-item>
                                    <el-dropdown-item>
                                        <el-button type="danger" icon="el-icon-Delete" size="small" @click.stop="confirmDelete(scope.row)" :loading="loading">删除</el-button>
                                    </el-dropdown-item>
                                </el-dropdown-menu>
                            </template>
                        </el-dropdown>
                    </template>
                </el-table-column>
            </el-table>

            <!-- 分页 -->
            <div class="pagination-container">
                <el-pagination v-model:current-page="currentPage"
                               v-model:page-size="pageSize"
                               :page-sizes="[10, 20, 50, 100]"
                               layout="total, sizes, prev, pager, next, jumper"
                               :total="total"
                               @size-change="handleSizeChange"
                               @current-change="handleCurrentChange"
                               :disabled="loading" />
            </div>
        </el-card>

        <!-- 添加/编辑预约模态框 -->
        <el-dialog :model-value="showAddModal || showEditModal"
                   @update:model-value="handleModalClose"
                   :title="showEditModal ? '编辑预约' : '添加预约'"
                   width="600px"
                   :close-on-click-modal="false"
                   @close="closeModal">
            <el-form ref="appointmentFormRef"
                     :model="formData"
                     label-width="100px"
                     label-position="left">
                <el-form-item label="患者ID" required>
                    <el-select v-model="formData.patientId"
                               placeholder="请选择患者"
                               style="width: 100%"
                               :loading="patientsLoading">
                        <el-option v-for="patient in patients"
                                   :key="patient.id"
                                   :label="`ID: ${patient.id} - ${patient.name || '未知患者'}`"
                                   :value="patient.id" />
                    </el-select>
                </el-form-item>
                <el-form-item label="牙医ID" required>
                    <el-select v-model="formData.dentistId"
                               placeholder="请选择牙医"
                               style="width: 100%"
                               :loading="dentistsLoading">
                        <el-option v-for="dentist in dentists"
                                   :key="dentist.id"
                                   :label="`ID: ${dentist.id} - ${dentist.name || '未知牙医'}`"
                                   :value="dentist.id" />
                    </el-select>
                </el-form-item>
                <el-form-item label="诊所ID" required>
                    <el-select v-model="formData.dentalOfficeId"
                               placeholder="请选择诊所"
                               style="width: 100%"
                               :loading="dentalOfficesLoading">
                        <el-option v-for="office in dentalOffices"
                                   :key="office.id"
                                   :label="`ID: ${office.id} - ${office.name || '未知诊所'}`"
                                   :value="office.id" />
                    </el-select>
                </el-form-item>
                <el-form-item label="开始时间" required>
                    <el-date-picker
                        v-model="formData.startDate"
                        type="datetime"
                        placeholder="选择开始时间"
                        style="width: 100%"
                    />
                </el-form-item>
                <el-form-item label="结束时间" required>
                    <el-date-picker
                        v-model="formData.endDate"
                        type="datetime"
                        placeholder="选择结束时间"
                        style="width: 100%"
                    />
                </el-form-item>
                <el-form-item label="状态" required>
                    <el-select v-model="formData.status" placeholder="选择状态" style="width: 100%">
                        <el-option label="待处理" :value="0" />
                        <el-option label="已确认" :value="1" />
                        <el-option label="已取消" :value="2" />
                    </el-select>
                </el-form-item>
            </el-form>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="closeModal">取消</el-button>
                    <el-button type="primary" @click="saveAppointment" :loading="loading">{{ loading ? '保存中...' : '保存' }}</el-button>
                </span>
            </template>
        </el-dialog>

        <!-- 删除确认模态框 -->
        <el-dialog v-model="showDeleteModal"
                   title="删除预约"
                   width="400px"
                   :close-on-click-modal="false"
                   @close="cancelDelete">
            <div>
                <p>确定要删除ID为"{{ deletingAppointment?.id }}"的预约吗？此操作不可恢复。</p>
            </div>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="cancelDelete">取消</el-button>
                    <el-button type="danger" @click="handleDeleteAppointment" :loading="loading">删除</el-button>
                </span>
            </template>
        </el-dialog>
    </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { ElMessage } from 'element-plus'
import { ArrowDown } from '@element-plus/icons-vue'
import { getAppointments, createAppointment, updateAppointment } from '../api/appointments'
import { getPatients } from '../api/patients'
import { getDentists } from '../api/dentists'
import { getDentalOffices } from '../api/dentalOffices'

// 处理模态框关闭
const handleModalClose = (val) => {
    if (!val) {
        closeModal()
    }
}

// 状态管理
const searchQuery = ref('')
const currentPage = ref(1)
const pageSize = ref(10)
const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingId = ref(null)
const deletingId = ref(null)
const loading = ref(false)
const total = ref(0)
const appointments = ref([])
const appointmentFormRef = ref(null)

// 下拉选择数据
const patients = ref([])
const dentists = ref([])
const dentalOffices = ref([])
const patientsLoading = ref(false)
const dentistsLoading = ref(false)
const dentalOfficesLoading = ref(false)

// 表单数据
const formData = reactive({
    id: '',
    patientId: '',
    dentistId: '',
    dentalOfficeId: '',
    startDate: null,  // 初始值改为 null
    endDate: null,    // 初始值改为 null
    status: 0
})

// 计算属性
const deletingAppointment = () => {
    return appointments.value.find(appointment => appointment.id === deletingId.value)
}

// 生命周期钩子
onMounted(() => {
    loadAppointments()
})

// 方法
const resetForm = () => {
    // 使用新的对象替换，确保Vue检测到变更
    Object.assign(formData, {
        id: '',
        patientId: '',
        dentistId: '',
        dentalOfficeId: '',
        startDate: null,  // 与初始化保持一致，使用 null
        endDate: null,    // 与初始化保持一致，使用 null
        status: 0
    })
    editingId.value = null
    // 强制触发更新：先将属性设为不同的值，再设回 null
    // 这样可以确保Vue检测到变化
    const tempValue = 'temp_reset_value_' + Date.now()
    formData.patientId = tempValue
    formData.dentistId = tempValue
    formData.dentalOfficeId = tempValue
    formData.status = -1 // 使用不同的值
    // 然后设回正确的初始值
    formData.patientId = ''
    formData.dentistId = ''
    formData.dentalOfficeId = ''
    formData.status = 0
    formData.startDate = new Date('2000-01-01') // 临时值
    formData.endDate = new Date('2000-01-01')   // 临时值
    formData.startDate = null
    formData.endDate = null
}

const addAppointment = () => {
    showAddModal.value = true
    showEditModal.value = false
    resetForm()
    // 加载下拉选择数据
    loadPatients()
    loadDentists()
    loadDentalOffices()
}


const closeModal = () => {
    showAddModal.value = false
    showEditModal.value = false
    resetForm()
}

// 格式化日期时间
const formatDateTime = (dateTime) => {
    if (!dateTime) return ''
    const date = new Date(dateTime)
    return date.toLocaleString()
}

// 获取状态文本
const getStatusText = (status) => {
    const statusMap = {
        0: '待处理',
        1: '已确认',
        2: '已取消'
    }
    return statusMap[status] || '未知'
}

// 获取状态类型
const getStatusType = (status) => {
    const typeMap = {
        0: 'warning',
        1: 'success',
        2: 'danger'
    }
    return typeMap[status] || 'info'
}

// 从后端加载预约数据
const loadAppointments = async () => {
    try {
        loading.value = true
        const response = await getAppointments({
            Page: currentPage.value,
            PageSize: pageSize.value,
            keyWord: searchQuery.value
        })
        appointments.value = response.data.data || []
        total.value = response.data.total || 0
    } catch (error) {
        console.error('加载预约失败:', error)
        ElMessage.error('加载预约列表失败，请刷新页面重试')
    } finally {
        loading.value = false
    }
}

// 加载患者列表
const loadPatients = async () => {
    try {
        patientsLoading.value = true
        const response = await getPatients({
            Page: 1,
            PageSize: 100
        })
        patients.value = response.data.data || []
    } catch (error) {
        console.error('加载患者列表失败:', error)
        // 不显示错误提示，避免影响用户体验
    } finally {
        patientsLoading.value = false
    }
}

// 加载牙医列表
const loadDentists = async () => {
    try {
        dentistsLoading.value = true
        const response = await getDentists({
            Page: 1,
            PageSize: 100
        })
        dentists.value = response.data.data || []
    } catch (error) {
        console.error('加载牙医列表失败:', error)
        // 不显示错误提示，避免影响用户体验
    } finally {
        dentistsLoading.value = false
    }
}

// 加载诊所列表
const loadDentalOffices = async () => {
    try {
        dentalOfficesLoading.value = true
        const response = await getDentalOffices({
            Page: 1,
            PageSize: 100
        })
        dentalOffices.value = response.data.data || []
    } catch (error) {
        console.error('加载诊所列表失败:', error)
        // 不显示错误提示，避免影响用户体验
    } finally {
        dentalOfficesLoading.value = false
    }
}

// 搜索
const onSearch = () => {
    currentPage.value = 1
    loadAppointments()
}

// 处理分页大小变化
const handleSizeChange = (newSize) => {
    pageSize.value = newSize
    currentPage.value = 1
    loadAppointments()
}

// 处理当前页码变化
const handleCurrentChange = (newCurrent) => {
    currentPage.value = newCurrent
    loadAppointments()
}

// 编辑预约
const editAppointment = (appointment) => {
    editingId.value = appointment.id
    Object.assign(formData, appointment)
    showEditModal.value = true
    // 加载下拉选择数据
    loadPatients()
    loadDentists()
    loadDentalOffices()
}

// 确认删除
const confirmDelete = (appointment) => {
    deletingId.value = appointment.id
    showDeleteModal.value = true
}

// 取消删除
const cancelDelete = () => {
    showDeleteModal.value = false
    deletingId.value = null
}

// 删除预约

// 保存预约（添加或编辑）
const saveAppointment = async () => {
    try {
        loading.value = true

        // 处理日期格式，确保转换为YYYY-MM-DD HH:mm:ss字符串
        const formatDateString = (date) => {
            if (!date) return null;
            if (typeof date === 'string') return date; // 如果已经是字符串，直接返回
            // 如果是Date对象，转换为YYYY-MM-DD HH:mm:ss格式
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const hours = String(date.getHours()).padStart(2, '0');
            const minutes = String(date.getMinutes()).padStart(2, '0');
            const seconds = String(date.getSeconds()).padStart(2, '0');
            return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
        };

        // 直接使用格式化后的字符串，避免时区转换问题
        const submitData = {
            ...formData,
            startDate: formatDateString(formData.startDate),
            endDate: formatDateString(formData.endDate)
        };

        if (editingId.value) {
            // 编辑模式
            await updateAppointment(editingId.value, submitData);
            ElMessage.success('更新预约成功');
        } else {
            // 添加模式
            await createAppointment(submitData);
            ElMessage.success('添加预约成功');
        }
        closeModal();
        loadAppointments();
    } catch (error) {
        console.error('保存预约失败:', error);
        ElMessage.error(error.response?.data?.message || '保存预约失败，请重试');
    } finally {
        loading.value = false;
    }
}
</script>

<style scoped>
.appointments-page {
    padding: 0;
}

.toolbar-card {
    margin-bottom: 20px;
}

.table-card {
    margin-bottom: 20px;
}

.toolbar {
    display: flex;
    align-items: center;
}

.pagination-container {
    margin-top: 20px;
    display: flex;
    justify-content: flex-end;
}

.dialog-footer {
    display: flex;
    justify-content: flex-end;
}

/* Element Plus 日期时间选择器已与系统样式保持一致，无需额外样式修复 */
</style>



















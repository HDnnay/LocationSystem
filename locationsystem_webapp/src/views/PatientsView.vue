<template>
    <div class="patients-page">
        <el-page-header>
            <template #title>
                <h1>患者管理</h1>
            </template>
        </el-page-header>

        <!-- 工具栏 -->
        <el-card class="toolbar-card" shadow="hover">
            <div class="toolbar">
                <el-input v-model="searchQuery"
                          placeholder="搜索姓名或邮箱"
                          clearable
                          style="width: 300px; margin-right: 10px;"
                          @keyup.enter="onSearch">
                    <template #append>
                        <el-button icon="el-icon-Search" @click="onSearch" :loading="loading">搜索</el-button>
                    </template>
                </el-input>
                <el-button type="primary" icon="el-icon-Plus" @click="addPatient" :loading="loading">添加患者</el-button>
            </div>
        </el-card>

        <!-- 患者列表 -->
        <el-card class="table-card" shadow="hover">
            <el-table v-loading="loading"
                      :data="patients"
                      style="width: 100%"
                      stripe
                      border>
                <el-table-column type="index"
                                 :index="(index) => (currentPage - 1) * pageSize + index + 1"
                                 label="序号"
                                 width="80" />

                <el-table-column prop="name" label="姓名" min-width="100" />
                <el-table-column prop="email" label="邮箱" min-width="180" />
                <el-table-column prop="createTime" label="创建时间" min-width="150">
                    <template #default="scope">
                        {{ scope.row.createTime || new Date().toISOString().split('T')[0] }}
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
                                        <el-button type="primary" icon="el-icon-Edit" size="small" @click.stop="editPatient(scope.row)" :loading="loading">编辑</el-button>
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

        <!-- 添加/编辑患者模态框 -->
        <el-dialog :model-value="showAddModal || showEditModal"
                   @update:model-value="val => this.handleModalClose(val)"
                   :title="showEditModal ? '编辑患者' : '添加患者'"
                   width="500px"
                   :close-on-click-modal="false"
                   @close="closeModal">
            <el-form ref="patientFormRef"
                     :model="formData"
                     label-width="80px"
                     label-position="left">
                <el-form-item label="姓名" required>
                    <el-input v-model="formData.name"
                              placeholder="请输入姓名"
                              maxlength="50"
                              show-word-limit />
                </el-form-item>
                <el-form-item label="邮箱" required :error="emailError">
                    <el-input v-model="formData.email"
                              placeholder="请输入邮箱"
                              type="email"
                              maxlength="100"
                              show-word-limit @change="validateEmail" />
                </el-form-item>
            </el-form>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="closeModal">取消</el-button>
                    <el-button type="primary" @click="savePatient" :loading="loading">{{ loading ? '保存中...' : '保存' }}</el-button>
                </span>
            </template>
        </el-dialog>

        <!-- 删除确认模态框 -->
        <el-dialog v-model="showDeleteModal"
                   title="删除患者"
                   width="400px"
                   :close-on-click-modal="false"
                   @close="cancelDelete">
            <div>
                <p>确定要删除患者"{{ deletingPatient?.name }}"吗？此操作不可恢复。</p>
            </div>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="cancelDelete">取消</el-button>
                    <el-button type="danger" @click="deletePatient" :loading="loading">删除</el-button>
                </span>
            </template>
        </el-dialog>
    </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { ElMessage } from 'element-plus'
import { ArrowDown } from '@element-plus/icons-vue'
import { getPatients, createPatient, updatePatient, deletePatient } from '../api/patients'

// 状态管理
const searchQuery = ref('')
const currentPage = ref(1)
const pageSize = ref(10)
const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingId = ref(null)
const deletingId = ref(null)
const emailError = ref('')
const loading = ref(false)
const total = ref(0)
const patients = ref([])
const patientFormRef = ref(null)

// 表单数据
const formData = reactive({
    id: '',
    name: '',
    email: ''
})

// 计算属性
const deletingPatient = () => {
    return patients.value.find(patient => patient.id === deletingId.value)
}

// 生命周期钩子
onMounted(() => {
    loadPatients()
})

// 方法
const resetForm = () => {
    Object.assign(formData, {
        id: '',
        name: '',
        email: ''
    })
    emailError.value = ''
    editingId.value = null
}

const addPatient = () => {
    showAddModal.value = true
    showEditModal.value = false
    resetForm()
}


const closeModal = () => {
    showAddModal.value = false
    showEditModal.value = false
    resetForm()
}

// 从后端加载患者数据
const loadPatients = async () => {
    try {
        loading.value = true
        const response = await getPatients({
            Page: currentPage.value,
            PageSize: pageSize.value,
            keyWord: searchQuery.value
        })
        patients.value = response.data.data || []
        total.value = response.data.total || 0
    } catch (error) {
        console.error('加载患者失败:', error)
        ElMessage.error('加载患者列表失败，请刷新页面重试')
    } finally {
        loading.value = false
    }
}

// 搜索
const onSearch = () => {
    currentPage.value = 1
    loadPatients()
}

// 处理分页大小变化
const handleSizeChange = (newSize) => {
    pageSize.value = newSize
    currentPage.value = 1
    loadPatients()
}

// 处理当前页码变化
const handleCurrentChange = (newCurrent) => {
    currentPage.value = newCurrent
    loadPatients()
}

// 验证邮箱格式
const validateEmail = (email) => {
    if (!email.trim()) {
        emailError.value = ''
        return true
    }

    // 验证邮箱格式
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (!emailRegex.test(email)) {
        emailError.value = '邮箱格式不正确'
        return false
    }
    emailError.value = ''
    return true
}

// 编辑患者
const editPatient = (patient) => {
    editingId.value = patient.id
    Object.assign(formData, patient)
    emailError.value = ''
    showEditModal.value = true
}

// 确认删除
const confirmDelete = (patient) => {
    deletingId.value = patient.id
    showDeleteModal.value = true
}

// 取消删除
const cancelDelete = () => {
    showDeleteModal.value = false
    deletingId.value = null
}

// 删除患者

// 保存患者（添加或编辑）
const savePatient = async () => {
    if (!validateEmail(formData.email)) {
        return
    }

    try {
        loading.value = true
        if (editingId.value) {
            // 编辑模式
            await updatePatient(editingId.value, formData)
            ElMessage.success('更新患者成功')
        } else {
            // 添加模式
            await createPatient(formData)
            ElMessage.success('添加患者成功')
        }
        closeModal()
        loadPatients()
    } catch (error) {
        console.error('保存患者失败:', error)
        ElMessage.error(error.response?.data?.message || '保存患者失败，请重试')
    } finally {
        loading.value = false
    }
}
</script>

<style scoped>
.patients-page {
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
</style>

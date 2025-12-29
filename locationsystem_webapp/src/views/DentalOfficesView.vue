<template>
    <div class="dental-offices-page">
        <el-page-header>
            <template #title>
                <h1>牙科诊所管理</h1>
            </template>
        </el-page-header>

        <!-- 工具栏 -->
        <el-card class="toolbar-card" shadow="hover">
            <div class="toolbar">
                <el-input v-model="searchQuery"
                          placeholder="搜索诊所名称"
                          clearable
                          style="width: 300px; margin-right: 10px;"
                          @keyup.enter="onSearch">
                    <template #append>
                        <el-button icon="el-icon-Search" @click="onSearch" :loading="loading">搜索</el-button>
                    </template>
                </el-input>
                <el-button type="primary" icon="el-icon-Plus" @click="addDentalOffice" :loading="loading">添加诊所</el-button>
            </div>
        </el-card>

        <!-- 诊所列表 -->
        <el-card class="table-card" shadow="hover">
            <el-table v-loading="loading"
                      :data="dentalOffices"
                      style="width: 100%"
                      stripe
                      border>
                <el-table-column type="index"
                                 :index="(index) => (currentPage - 1) * pageSize + index + 1"
                                 label="序号"
                                 width="80" />

                <el-table-column prop="name" label="诊所名称" min-width="200" />
                <el-table-column label="操作" min-width="100" fixed="right">
                    <template #default="scope">
                        <el-dropdown>
                            <el-button type="primary" size="small">
                                操作 <el-icon class="el-icon--right"><ArrowDown /></el-icon>
                            </el-button>
                            <template #dropdown>
                                <el-dropdown-menu>
                                    <el-dropdown-item>
                                        <el-button type="primary" icon="el-icon-Edit" size="small" @click.stop="editDentalOffice(scope.row)" :loading="loading">编辑</el-button>
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

        <!-- 添加/编辑诊所模态框 -->
        <el-dialog :model-value="showAddModal || showEditModal"
                   @update:model-value="val => this.handleModalClose(val)"
                   :title="showEditModal ? '编辑诊所' : '添加诊所'"
                   width="500px"
                   :close-on-click-modal="false"
                   @close="closeModal">
            <el-form ref="dentalOfficeFormRef"
                     :model="formData"
                     label-width="80px"
                     label-position="left">
                <el-form-item label="诊所名称" required>
                    <el-input v-model="formData.name"
                              placeholder="请输入诊所名称"
                              maxlength="100"
                              show-word-limit />
                </el-form-item>
            </el-form>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="closeModal">取消</el-button>
                    <el-button type="primary" @click="saveDentalOffice" :loading="loading">{{ loading ? '保存中...' : '保存' }}</el-button>
                </span>
            </template>
        </el-dialog>

        <!-- 删除确认模态框 -->
        <el-dialog v-model="showDeleteModal"
                   title="删除诊所"
                   width="400px"
                   :close-on-click-modal="false"
                   @close="cancelDelete">
            <div>
                <p>确定要删除诊所"{{ deletingDentalOffice?.name }}"吗？此操作不可恢复。</p>
            </div>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="cancelDelete">取消</el-button>
                    <el-button type="danger" @click="handleDeleteDentalOffice" :loading="loading">删除</el-button>
                </span>
            </template>
        </el-dialog>
    </div>
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'
import { ElMessage } from 'element-plus'
import { ArrowDown } from '@element-plus/icons-vue'
import { getDentalOffices, createDentalOffice, updateDentalOffice, deleteDentalOffice } from '../api/dentalOffices'

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
const dentalOffices = ref([])
const dentalOfficeFormRef = ref(null)

// 表单数据
const formData = reactive({
    id: '',
    name: ''
})

// 计算属性
const deletingDentalOffice = () => {
    return dentalOffices.value.find(office => office.id === deletingId.value)
}

// 生命周期钩子
onMounted(() => {
    loadDentalOffices()
})

// 方法
const resetForm = () => {
    Object.assign(formData, {
        id: '',
        name: ''
    })
    editingId.value = null
}

const addDentalOffice = () => {
    showAddModal.value = true
    showEditModal.value = false
    resetForm()
}


const closeModal = () => {
    showAddModal.value = false
    showEditModal.value = false
    resetForm()
}

// 从后端加载诊所数据
const loadDentalOffices = async () => {
    try {
        loading.value = true
        const response = await getDentalOffices({
            Page: currentPage.value,
            PageSize: pageSize.value,
            keyWord: searchQuery.value
        })
        dentalOffices.value = response.data.data || []
        total.value = response.data.total || 0
    } catch (error) {
        console.error('加载诊所失败:', error)
        ElMessage.error('加载诊所列表失败，请刷新页面重试')
    } finally {
        loading.value = false
    }
}

// 搜索
const onSearch = () => {
    currentPage.value = 1
    loadDentalOffices()
}

// 处理分页大小变化
const handleSizeChange = (newSize) => {
    pageSize.value = newSize
    currentPage.value = 1
    loadDentalOffices()
}

// 处理当前页码变化
const handleCurrentChange = (newCurrent) => {
    currentPage.value = newCurrent
    loadDentalOffices()
}

// 编辑诊所
const editDentalOffice = (office) => {
    editingId.value = office.id
    Object.assign(formData, office)
    showEditModal.value = true
}

// 确认删除
const confirmDelete = (office) => {
    deletingId.value = office.id
    showDeleteModal.value = true
}

// 取消删除
const cancelDelete = () => {
    showDeleteModal.value = false
    deletingId.value = null
}

// 删除诊所
const handleDeleteDentalOffice = async () => {
    if (deletingId.value) {
        try {
            await deleteDentalOffice(deletingId.value)
            ElMessage.success('删除诊所成功')
            cancelDelete()
            // 删除成功后重新加载数据
            loadDentalOffices()
        } catch (error) {
            console.error('删除诊所失败:', error)
            ElMessage.error(error.response?.data?.message || '删除诊所失败，请重试')
            cancelDelete()
        }
    }
}

// 保存诊所（添加或编辑）
const saveDentalOffice = async () => {
    try {
        loading.value = true

        if (editingId.value) {
            // 编辑模式
            await updateDentalOffice(editingId.value, formData)
            ElMessage.success('更新诊所成功')
        } else {
            // 添加模式
            await createDentalOffice(formData)
            ElMessage.success('添加诊所成功')
        }
        closeModal()
        loadDentalOffices()
    } catch (error) {
        console.error('保存诊所失败:', error)
        ElMessage.error(error.response?.data?.message || '保存诊所失败，请重试')
    } finally {
        loading.value = false
    }
}

// 处理模态框关闭
const handleModalClose = (val) => {
    if (!val) {
        closeModal()
    }
}
</script>

<style scoped>
.dental-offices-page {
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



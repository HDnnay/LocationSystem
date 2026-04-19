<template>
  <div class="user-table">
    <!-- 操作按钮 -->
    <div class="action-buttons" v-if="!isDeletedTable">
      <el-button type="primary" @click="handleCreate">
        <el-icon><Plus /></el-icon>
        {{ createButtonText }}
      </el-button>
    </div>

    <!-- 表格 -->
    <el-table :data="tableData" style="width: 100%; margin-top: 20px" stripe border>
      <el-table-column label="序号" width="80">
        <template #default="scope">
          {{ (currentPage - 1) * pageSize + scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column prop="name" label="用户名" />
      <el-table-column prop="email" label="邮箱" />
      <el-table-column prop="userType" label="用户类型" />
      <el-table-column prop="createTime" label="创建时间">
            <template #default="scope">
              {{ new Date(scope.row.createTime).toLocaleString() }}
            </template>
          </el-table-column>
          <el-table-column prop="deleteTime" label="删除时间" v-if="isDeletedTable">
            <template #default="scope">
              {{ new Date(scope.row.deleteTime).toLocaleString() }}
            </template>
          </el-table-column>
          <el-table-column prop="isDelete" label="已删除" v-if="isDeletedTable">
                    <template #default="scope">
                      <el-tag :type="scope.row.isDelete ? 'danger' : 'success'">
                        {{ scope.row.isDelete ? '是' : '否' }}
                      </el-tag>
                    </template>
          </el-table-column>
          <el-table-column prop="isDisabled" label="禁用" v-if="!isDeletedTable">
            <template #default="scope">
                        <el-tag :type="scope.row.isDisabled ? 'danger' : 'success'">
                           {{ scope.row.isDisabled ? '是' : '否' }}
                        </el-tag>
                    </template>
          </el-table-column>
      <el-table-column label="角色" width="200">
        <template #default="scope">
          <el-tag v-for="role in scope.row.roles" :key="role.id" size="small" style="margin-right: 5px">
            {{ role.name }}
          </el-tag>
          <span v-if="!scope.row.roles || scope.row.roles.length === 0" class="text-gray-400">无</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="300">
        <template #default="scope">
          <el-button size="small" type="primary" @click="handleEdit(scope.row)" style="margin-right: 5px" v-if="!isDeletedTable">
            <el-icon><Edit /></el-icon>
            编辑
          </el-button>
          <el-button size="small" type="danger" @click="handleDelete(scope.row.id)" style="margin-right: 5px" v-if="!isDeletedTable">
            <el-icon><Delete /></el-icon>
            删除
          </el-button>
          <el-button size="small" type="warning" @click="handleAssignRoles(scope.row)" v-if="!isDeletedTable">
            <el-icon><SetUp /></el-icon>
            分配角色
          </el-button>
          <el-button size="small" type="success" @click="handleRestore(scope.row.id)" v-if="isDeletedTable">
            <el-icon><RefreshRight /></el-icon>
            恢复
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 分页 -->
    <div class="pagination">
      <el-pagination
        v-model:current-page="currentPage"
        v-model:page-size="pageSize"
        :page-sizes="[10, 20, 50, 100]"
        layout="total, sizes, prev, pager, next, jumper"
        :total="total"
        @size-change="handleSizeChange"
        @current-change="handleCurrentChange"
      />
    </div>

    <!-- 编辑对话框 -->
    <el-dialog v-model="dialogVisible" :title="dialogTitle" width="500px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="用户名">
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="form.email" />
        </el-form-item>
        <el-form-item v-if="!isEdit" label="用户类型">
          <el-select v-model="form.userType">
            <el-option v-for="type in userTypes" :key="type.value" :label="type.name" :value="type.value" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSave">保存</el-button>
        </span>
      </template>
    </el-dialog>

    <!-- 分配角色对话框 -->
    <el-dialog v-model="roleDialogVisible" title="分配角色" width="500px">
      <el-form label-width="80px">
        <el-form-item label="选择角色">
          <el-checkbox-group v-model="selectedRoles">
            <el-checkbox v-for="role in roles" :key="role.id" :label="role.id">
              {{ role.name }}
            </el-checkbox>
          </el-checkbox-group>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="roleDialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSaveRoles">保存</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { ref, watch, onMounted, computed } from 'vue'
import { Plus, Edit, Delete, SetUp, RefreshRight } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from '@/api'
import { getUserTypes } from '@/api/users'

export default {
  name: 'UserTable',
  components: {
    Plus,
    Edit,
    Delete,
    SetUp,
    RefreshRight
  },
  props: {
    tableData: {
      type: Array,
      default: () => []
    },
    total: {
      type: Number,
      default: 0
    },
    currentPage: {
      type: Number,
      default: 1
    },
    pageSize: {
      type: Number,
      default: 10
    },
    type: {
      type: String,
      default: 'normal'
    },
    createButtonText: {
      type: String,
      default: '新增用户'
    }
  },
  emits: ['update:currentPage', 'update:pageSize', 'refresh'],
  setup(props, { emit }) {
    const currentPage = ref(props.currentPage)
    const pageSize = ref(props.pageSize)

    const dialogVisible = ref(false)
    const roleDialogVisible = ref(false)
    const dialogTitle = ref('新增用户')
    const form = ref({})
    const isEdit = ref(false)
    const currentEditId = ref(null)

    const roles = ref([])
    const userTypes = ref([])
    const selectedRoles = ref([])
    const currentUser = ref(null)

    watch(() => props.currentPage, (val) => {
      currentPage.value = val
    })

    watch(() => props.pageSize, (val) => {
      pageSize.value = val
    })

    const isDeletedTable = computed(() => props.type === 'deleted')

    const loadUserTypes = async () => {
      try {
        const response = await getUserTypes()
        if (response && response.data) {
          userTypes.value = response.data
        }
      } catch (error) {
        console.error('加载用户类型失败:', error)
      }
    }

    const loadRoles = async () => {
      try {
        const response = await api.roles.getAllRoles()
        if (response.status === 200) {
          roles.value = response.data || []
        }
      } catch (error) {
        console.error('加载角色失败:', error)
      }
    }

    const handleCreate = () => {
      dialogTitle.value = '新增用户'
      isEdit.value = false
      currentEditId.value = null
      form.value = {
        name: '',
        email: '',
        userType: ''
      }
      dialogVisible.value = true
    }

    const handleEdit = (row) => {
      dialogTitle.value = '编辑用户'
      isEdit.value = true
      currentEditId.value = row.id
      form.value = {
        ...row,
        email: row.email.value || row.email
      }
      dialogVisible.value = true
    }

    const handleSave = async () => {
      if (!form.value.name || !form.value.email || (!isEdit.value && !form.value.userType)) {
        ElMessage.warning('请填写完整的用户信息')
        return
      }
      try {
        const userData = {
          Name: form.value.name,
          Email: form.value.email,
          UserType: form.value.userType?.toString() || ''
        }

        let response
        if (isEdit.value) {
          response = await api.users.updateUser(currentEditId.value, userData)
        } else {
          response = await api.users.createUser(userData)
        }

        if (response.status === 200) {
          ElMessage.success(isEdit.value ? '更新用户成功' : '新增用户成功')
          dialogVisible.value = false
          emit('refresh')
        } else {
          ElMessage.error(isEdit.value ? '更新用户失败' : '新增用户失败')
        }
      } catch (error) {
        console.error('保存用户失败:', error)
        const errorMsg = error.response?.data?.message || error.message || '保存用户失败'
        ElMessage.error('保存用户失败: ' + errorMsg)
      }
    }

    const handleDelete = async (id) => {
      try {
        await ElMessageBox.confirm('确定要删除这个用户吗？', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        })

        const response = await api.users.deleteUser(id)
        if (response.status === 200) {
          ElMessage.success('删除用户成功')
          emit('refresh')
        } else {
          ElMessage.error('删除用户失败')
        }
      } catch (error) {
        if (error !== 'cancel') {
          console.error('删除用户失败:', error)
          ElMessage.error('删除用户失败')
        }
      }
    }

    const handleRestore = async (id) => {
      try {
        await ElMessageBox.confirm('确定要恢复这个用户吗？', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        })

        const response = await api.users.restoreUser(id)
        if (response.status === 200) {
          ElMessage.success('恢复用户成功')
          emit('refresh')
        } else {
          ElMessage.error('恢复用户失败')
        }
      } catch (error) {
        if (error !== 'cancel') {
          console.error('恢复用户失败:', error)
          ElMessage.error('恢复用户失败')
        }
      }
    }

    const handleAssignRoles = (user) => {
      currentUser.value = user
      selectedRoles.value = user.roles?.map(role => role.id) || []
      loadRoles()
      roleDialogVisible.value = true
    }

    const handleSaveRoles = async () => {
      try {
        const response = await api.users.assignRoles(currentUser.value.id, selectedRoles.value)
        if (response.status === 200) {
          ElMessage.success('分配角色成功')
          roleDialogVisible.value = false
          emit('refresh')
        } else {
          ElMessage.error('分配角色失败')
        }
      } catch (error) {
        console.error('分配角色失败:', error)
        ElMessage.error('分配角色失败')
      }
    }

    const handleSizeChange = (size) => {
      emit('update:pageSize', size)
      emit('refresh', { currentPage: currentPage.value, pageSize: size })
    }

    const handleCurrentChange = (current) => {
      emit('update:currentPage', current)
      emit('refresh', { currentPage: current, pageSize: pageSize.value })
    }

    onMounted(() => {
      loadUserTypes()
    })

    return {
      currentPage,
      pageSize,
      dialogVisible,
      roleDialogVisible,
      dialogTitle,
      form,
      isEdit,
      roles,
      userTypes,
      selectedRoles,
      isDeletedTable,
      handleCreate,
      handleEdit,
      handleSave,
      handleDelete,
      handleRestore,
      handleAssignRoles,
      handleSaveRoles,
      handleSizeChange,
      handleCurrentChange
    }
  }
}
</script>

<style scoped>
.user-table {
  width: 100%;
}

.action-buttons {
  margin-bottom: 20px;
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: flex-end;
}

.text-gray-400 {
  color: #909399;
}
</style>


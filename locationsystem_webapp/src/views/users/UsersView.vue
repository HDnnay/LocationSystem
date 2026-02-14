<template>
  <div class="users-view">
    <h1>用户管理</h1>

    <!-- 操作按钮 -->
    <div class="action-buttons">
      <el-button type="primary" @click="handleCreateUser">
        <el-icon><Plus /></el-icon>
        新增用户
      </el-button>
    </div>

    <!-- 用户列表 -->
    <el-table :data="users" style="width: 100%">
      <el-table-column label="序号" width="180">
        <template #default="scope">
          {{ (currentPage - 1) * pageSize + scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column prop="name" label="用户名" />
      <el-table-column prop="email" label="邮箱" />
      <el-table-column prop="userType" label="用户类型" />
      <el-table-column label="角色" width="200">
        <template #default="scope">
          <el-tag v-for="role in scope.row.roles" :key="role.id" size="small" style="margin-right: 5px">
            {{ role.name }}
          </el-tag>
          <span v-if="scope.row.roles.length === 0" class="text-gray-400">无</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template #default="scope">
          <el-button size="small" @click="handleEditUser(scope.row)">
            <el-icon><Edit /></el-icon>
            编辑
          </el-button>
          <el-button size="small" type="danger" @click="handleDeleteUser(scope.row.id)">
            <el-icon><Delete /></el-icon>
            删除
          </el-button>
          <el-button size="small" type="warning" @click="handleAssignRoles(scope.row)">
            <el-icon><SetUp /></el-icon>
            分配角色
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

    <!-- 编辑用户对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="500px"
    >
      <el-form :model="form" label-width="80px">
        <el-form-item label="用户名">
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="form.email" />
        </el-form-item>
        <el-form-item label="用户类型">
          <el-select v-model="form.userType">
            <el-option v-for="type in userTypes" :key="type.value" :label="type.name" :value="type.value" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSaveUser">保存</el-button>
        </span>
      </template>
    </el-dialog>

    <!-- 分配角色对话框 -->
    <el-dialog
      v-model="roleDialogVisible"
      title="分配角色"
      width="500px"
    >
      <el-form label-width="80px">
        <el-form-item label="选择角色">
          <el-checkbox-group v-model="selectedRoles">
            <el-checkbox
              v-for="role in roles"
              :key="role.id"
              :label="role.id"
            >
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
import { ref, onMounted } from 'vue'
import { Plus, Edit, Delete, SetUp } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from '@/api'
import { getUserTypes } from '@/api/users'

export default {
  name: 'UsersView',
  components: {
    Plus,
    Edit,
    Delete,
    SetUp
  },
  setup() {
    const users = ref([])
    const roles = ref([])
    const userTypes = ref([])
    const currentPage = ref(1)
    const pageSize = ref(10)
    const total = ref(0)
    const dialogVisible = ref(false)
    const roleDialogVisible = ref(false)
    const dialogTitle = ref('新增用户')
    const form = ref({})
    const selectedRoles = ref([])
    const currentUser = ref(null)

    // 加载用户列表
    const loadUsers = async () => {
      try {
        const response = await api.users.getAllUsers()
        users.value = response.data || []
        total.value = response.total
      } catch (error) {
        console.error('加载用户列表失败:', error)
        ElMessage.error('加载用户列表失败')
      }
    }

    // 加载角色列表
    const loadRoles = async () => {
      try {
        // 调试：检查api.roles对象
        console.log('api.roles:', api.roles)
        console.log('api.roles.getAllRoles:', api.roles.getAllRoles)

        // 确保getAllRoles是一个函数
        if (typeof api.roles.getAllRoles !== 'function') {
          throw new Error('api.roles.getAllRoles不是一个函数')
        }

        const response = await api.roles.getAllRoles()
        console.log('获取到的角色数据:', response)
        // 检查响应数据结构
        if (Array.isArray(response)) {
          roles.value = response
        } else if (response && Array.isArray(response.data)) {
          roles.value = response.data
        } else {
          roles.value = []
          console.error('角色数据格式错误:', response)
        }
      } catch (error) {
        console.error('加载角色列表失败:', error)
        // 显示具体的错误信息
        const errorMessage = error.response?.data?.message || error.message || '加载角色列表失败'
        ElMessage.error(errorMessage)
        // 如果是403错误，可能是权限不足
        if (error.response?.status === 403) {
          ElMessage.warning('您没有查看角色的权限')
        }
        roles.value = []
      }
    }

    // 加载用户类型列表
    const loadUserTypes = async () => {
      try {
        const response = await getUserTypes()
        console.log('用户类型数据:', response)
        userTypes.value = Array.isArray(response) ? response : (response.data || [])
      } catch (error) {
        console.error('加载用户类型列表失败:', error)
        ElMessage.error('加载用户类型列表失败')
        // 如果获取失败，使用默认值
        userTypes.value = [
          { value: 0, name: '默认用户' },
          { value: 1, name: '管理员' },
          { value: 2, name: '普通用户' }
        ];
      }
    }

    // 处理新增用户
    const handleCreateUser = () => {
      dialogTitle.value = '新增用户'
      form.value = {
        name: '',
        email: '',
        userType: 'Dentist'
      }
      dialogVisible.value = true
    }

    // 处理编辑用户
    const handleEditUser = (user) => {
      dialogTitle.value = '编辑用户'
      form.value = {
        ...user,
        email: user.email.value || user.email
      }
      dialogVisible.value = true
    }

    // 处理保存用户
    const handleSaveUser = async () => {
      try {
        if (form.value.id) {
          // 更新用户
          // 后端期望的格式：{ "Name": "lisi", "Email": "lisi@qq.com", "UserType": "Dentist" }
          const userData = {
            Name: form.value.name,
            Email: form.value.email,
            UserType: form.value.userType // 保持为字符串，如'Dentist'或'Patient'
          }
          await api.users.updateUser(form.value.id, userData)
          ElMessage.success('更新用户成功')
        } else {
          // 新增用户
          // 后端期望的格式：{ "Name": "lisi", "Email": "lisi@qq.com", "UserType": "Dentist" }
          const userData = {
            Name: form.value.name,
            Email: form.value.email,
            UserType: form.value.userType // 保持为字符串，如'Dentist'或'Patient'
          }
          await api.users.createUser(userData).then(res => {
            ElMessage.success('新增用户成功')
          })
        }
        dialogVisible.value = false
        loadUsers()
      } catch (error) {
        console.error('保存用户失败:', error)
        ElMessage.error('保存用户失败')
      }
    }

    // 处理删除用户
    const handleDeleteUser = async (id) => {
      try {
        await ElMessageBox.confirm('确定要删除这个用户吗？', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        })

        await api.users.deleteUser(id)
        ElMessage.success('删除用户成功')
        loadUsers()
      } catch (error) {
        if (error !== 'cancel') {
          console.error('删除用户失败:', error)
          ElMessage.error('删除用户失败')
        }
      }
    }

    // 处理分配角色
    const handleAssignRoles = (user) => {
      currentUser.value = user
      // 初始化选中的角色
      selectedRoles.value = user.roles.map(role => role.id)
      loadRoles()
      roleDialogVisible.value = true
    }

    // 处理保存角色
    const handleSaveRoles = async () => {
      try {
        await api.users.assignRoles(currentUser.value.id, selectedRoles.value)
        ElMessage.success('分配角色成功')
        roleDialogVisible.value = false
        loadUsers()
      } catch (error) {
        console.error('分配角色失败:', error)
        ElMessage.error('分配角色失败')
      }
    }

    // 处理分页大小变化
    const handleSizeChange = (size) => {
      pageSize.value = size
      loadUsers()
    }

    // 处理当前页变化
    const handleCurrentChange = (current) => {
      currentPage.value = current
      loadUsers()
    }

    // 页面加载时初始化数据
    onMounted(() => {
      loadUsers()
      loadRoles()
      loadUserTypes()
    })

    return {
      users,
      roles,
      userTypes,
      currentPage,
      pageSize,
      total,
      dialogVisible,
      roleDialogVisible,
      dialogTitle,
      form,
      selectedRoles,
      currentUser,
      handleCreateUser,
      handleEditUser,
      handleSaveUser,
      handleDeleteUser,
      handleAssignRoles,
      handleSaveRoles,
      handleSizeChange,
      handleCurrentChange
    }
  }
}
</script>

<style scoped>
.users-view {
  padding: 20px;
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

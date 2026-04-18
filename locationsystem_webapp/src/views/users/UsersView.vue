<template>
  <div class="users-view">
    <h1>用户管理</h1>

    <!-- Tab切换 -->
    <el-tabs v-model="activeTab" class="user-tabs">
      <el-tab-pane label="用户列表" name="active">
        <UserTable
          :table-data="users"
          :total="total"
          v-model:current-page="currentPage"
          v-model:page-size="pageSize"
          @refresh="handleRefresh"
        />
      </el-tab-pane>

      <el-tab-pane label="已删除用户" name="deleted">
        <!-- 已删除用户列表 -->
        <el-table :data="deletedUsers" style="width: 100%; margin-top: 20px" stripe border>
          <el-table-column label="序号" width="180">
            <template #default="scope">
              {{ (deletedCurrentPage - 1) * deletedPageSize + scope.$index + 1 }}
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
          <el-table-column prop="deleteTime" label="删除时间">
            <template #default="scope">
              {{ new Date(scope.row.deleteTime).toLocaleString() }}
            </template>
          </el-table-column>
          <el-table-column prop="isDelete" label="已删除">
                    <template #default="scope">
                      <el-tag :type="scope.row.isDelete ? 'danger' : 'success'">
                        {{ scope.row.isDelete ? '是' : '否' }}
                      </el-tag>
                    </template>
          </el-table-column>
          <el-table-column prop="isDisabled" label="禁用">
            <template #default="scope">
                        <el-tag :type="scope.row.isDisabled ? 'danger' : 'success'">
                           {{ scope.row.isDisabled ? '是' : '否' }}
                        </el-tag>
                    </template>
          </el-table-column>
          <el-table-column label="操作" width="150">
            <template #default="scope">
              <el-button size="small" type="success" @click="handleRestoreUser(scope.row.id)">
                <el-icon><Refresh /></el-icon>
                恢复
              </el-button>
            </template>
          </el-table-column>
        </el-table>

        <!-- 分页 -->
        <div class="pagination">
          <el-pagination
            v-model:current-page="deletedCurrentPage"
            v-model:page-size="deletedPageSize"
            :page-sizes="[10, 20, 50, 100]"
            layout="total, sizes, prev, pager, next, jumper"
            :total="deletedTotal"
            @size-change="handleDeletedSizeChange"
            @current-change="handleDeletedCurrentChange"
          />
        </div>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import { ref, onMounted, watch } from 'vue'
import { Refresh } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from '@/api'
import UserTable from '@/components/UserTable.vue'

export default {
  name: 'UsersView',
  components: {
    Refresh,
    UserTable
  },
  setup() {
    const users = ref([])
    const currentPage = ref(1)
    const pageSize = ref(10)
    const total = ref(0)

    const activeTab = ref('active')
    const deletedUsers = ref([])
    const deletedCurrentPage = ref(1)
    const deletedPageSize = ref(10)
    const deletedTotal = ref(0)

    watch(activeTab, (newTab) => {
      if (newTab === 'active') {
        loadUsers()
      } else {
        loadDeletedUsers()
      }
    })

    const loadUsers = async () => {
      try {
        const response = await api.users.getAllUsers()
        if (response.status === 200) {
          users.value = response.data.items || response.data || []
          total.value = response.data.total || 0
        } else {
          ElMessage.error('加载用户列表失败')
        }
      } catch (error) {
        console.error('加载用户列表失败:', error)
        ElMessage.error('加载用户列表失败')
      }
    }

    const loadDeletedUsers = async () => {
      try {
        const response = await api.users.getDeletedUsers()
        if (response.status === 200) {
          deletedUsers.value = response.data.items || response.data || []
          console.log("删除用户列表:", deletedUsers.value);
          deletedTotal.value = response.data.total || 0
        } else {
          ElMessage.error('加载已删除用户列表失败')
        }
      } catch (error) {
        console.error('加载已删除用户列表失败:', error)
        ElMessage.error('加载已删除用户列表失败')
      }
    }

    const handleRefresh = () => {
      loadUsers()
    }

    const handleRestoreUser = async (id) => {
      try {
        await ElMessageBox.confirm('确定要恢复这个用户吗？', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        })

        const response = await api.users.restoreUser(id)
        if (response.status === 200) {
          ElMessage.success('恢复用户成功')
          loadDeletedUsers()
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

    const handleDeletedSizeChange = (size) => {
      deletedPageSize.value = size
      loadDeletedUsers()
    }

    const handleDeletedCurrentChange = (current) => {
      deletedCurrentPage.value = current
      loadDeletedUsers()
    }

    onMounted(() => {
      loadUsers()
    })

    return {
      users,
      currentPage,
      pageSize,
      total,
      activeTab,
      deletedUsers,
      deletedCurrentPage,
      deletedPageSize,
      deletedTotal,
      handleRefresh,
      handleRestoreUser,
      handleDeletedSizeChange,
      handleDeletedCurrentChange
    }
  }
}
</script>

<style scoped>
.users-view {
  padding: 20px;
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: flex-end;
}

.user-tabs {
  margin-top: 20px;
}
</style>

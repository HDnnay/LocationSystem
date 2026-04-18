<template>
  <div class="users-view">
    <h1>用户管理</h1>

    <!-- Tab切换 -->
    <el-tabs v-model="activeTab" class="user-tabs">
      <el-tab-pane label="用户列表" name="active">
        <UserTable
          :table-data="users"
          :total="total"
          type="normal"
          v-model:current-page="currentPage"
          v-model:page-size="pageSize"
          @refresh="handleRefresh"
        />
      </el-tab-pane>

      <el-tab-pane label="已删除用户" name="deleted">
        <UserTable
          :table-data="deletedUsers"
          :total="deletedTotal"
          type="deleted"
          v-model:current-page="deletedCurrentPage"
          v-model:page-size="deletedPageSize"
          @refresh="handleDeletedRefresh"
        />
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import { ref, onMounted, watch } from 'vue'
import api from '@/api'
import UserTable from '@/components/UserTable.vue'

export default {
  name: 'UsersView',
  components: {
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
        }
      } catch (error) {
        console.error('加载用户列表失败:', error)
      }
    }

    const loadDeletedUsers = async () => {
      try {
        const response = await api.users.getDeletedUsers()
        if (response.status === 200) {
          deletedUsers.value = response.data.items || response.data || []
          deletedTotal.value = response.data.total || 0
        }
      } catch (error) {
        console.error('加载已删除用户列表失败:', error)
      }
    }

    const handleRefresh = () => {
      loadUsers()
    }

    const handleDeletedRefresh = () => {
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
      handleDeletedRefresh
    }
  }
}
</script>

<style scoped>
.users-view {
  padding: 20px;
}

.user-tabs {
  margin-top: 20px;
}
</style>

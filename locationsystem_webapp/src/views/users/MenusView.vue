<template>
  <div class="menus-view">
    <h1>菜单管理</h1>

    <!-- 操作按钮 -->
    <div class="action-buttons">
      <el-button type="primary" @click="handleCreateMenu">
        <el-icon><Plus /></el-icon>
        新增菜单
      </el-button>
    </div>

    <!-- 菜单列表 -->
    <el-table :data="menuList" style="width: 100%">
      <el-table-column prop="name" label="菜单名称" />
      <el-table-column prop="path" label="菜单路径" />
      <el-table-column prop="icon" label="菜单图标" />
      <el-table-column prop="order" label="排序" />
      <el-table-column label="父菜单" />
      <el-table-column label="操作" width="200">
        <template #default="scope">
          <el-button size="small" @click="handleEditMenu(scope.row)">
            <el-icon><Edit /></el-icon>
            编辑
          </el-button>
          <el-button size="small" type="danger" @click="handleDeleteMenu(scope.row.id)">
            <el-icon><Delete /></el-icon>
            删除
          </el-button>
          <el-button size="small" type="warning" @click="handleCreateSubMenu(scope.row)">
            <el-icon><Plus /></el-icon>
            子菜单
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

    <!-- 新增/编辑菜单对话框 -->
    <el-dialog
      v-model="dialogVisible"
      :title="dialogTitle"
      width="500px"
    >
      <el-form :model="form" label-width="80px">
        <el-form-item label="菜单名称">
          <el-input v-model="form.name" />
        </el-form-item>
        <el-form-item label="菜单路径">
          <el-input v-model="form.path" />
        </el-form-item>
        <el-form-item label="菜单图标">
          <el-input v-model="form.icon" />
        </el-form-item>
        <el-form-item label="排序">
          <el-input-number v-model="form.order" :min="1" :max="100" />
        </el-form-item>
        <el-form-item label="父菜单">
          <el-select v-model="form.parentId" placeholder="选择父菜单">
            <el-option label="无" value="" />
            <el-option
              v-for="menu in parentMenuOptions"
              :key="menu.id"
              :label="menu.name"
              :value="menu.id"
            />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSaveMenu">保存</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { Plus, Edit, Delete } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'

export default {
  name: 'MenusView',
  components: {
    Plus,
    Edit,
    Delete
  },
  setup() {
    const menus = ref([])
    const menuList = ref([])
    const dialogVisible = ref(false)
    const dialogTitle = ref('新增菜单')
    const form = ref({})
    const filterText = ref('')
    const currentPage = ref(1)
    const pageSize = ref(10)
    const total = ref(0)

    // 加载菜单列表
    const loadMenus = async () => {
      try {
        // 从后端获取菜单列表（带分页）
        const api = await import('@/api')
        const response = await api.default.menus.getAllMenus(currentPage.value, pageSize.value)
        // 直接使用response作为菜单列表，因为后端直接返回了菜单列表
        menus.value = response.items
        total.value = response.total
        // 直接使用后端返回的分页数据
        menuList.value = response.items
      } catch (error) {
        console.error('加载菜单列表失败:', error)
        // 使用模拟数据作为备选
        const mockMenus = [
          {
            id: '1',
            name: '角色权限管理',
            path: '/roles',
            icon: 'SetUp',
            order: 1,
            parentId: null
          },
          {
            id: '2',
            name: '角色管理',
            path: '/roles',
            icon: 'SetUp',
            order: 1,
            parentId: '1'
          },
          {
            id: '3',
            name: '权限管理',
            path: '/permissions',
            icon: 'Key',
            order: 2,
            parentId: '1'
          },
          {
            id: '4',
            name: '用户管理',
            path: '/users',
            icon: 'User',
            order: 3,
            parentId: '1'
          },
          {
            id: '5',
            name: '菜单管理',
            path: '/menus',
            icon: 'List',
            order: 4,
            parentId: '1'
          },
          {
            id: '6',
            name: '公司管理',
            path: '/company/list',
            icon: 'OfficeBuilding',
            order: 2,
            parentId: null
          },
          {
            id: '7',
            name: '租房管理',
            path: '/rent/list',
            icon: 'House',
            order: 3,
            parentId: null
          }
        ]

        // 直接使用模拟数据作为菜单列表
        menus.value = mockMenus
        total.value = menus.value.length
        // 计算当前页的菜单列表
        const startIndex = (currentPage.value - 1) * pageSize.value
        const endIndex = startIndex + pageSize.value
        menuList.value = menus.value.slice(startIndex, endIndex)

        if (error.response) {
          // 服务器返回了错误响应
          ElMessage.error(`加载菜单列表失败: ${error.response.data.message || '服务器内部错误'}`)
        } else if (error.request) {
          // 请求已发出，但没有收到响应
          ElMessage.error('加载菜单列表失败: 服务器无响应')
        } else {
          // 请求配置出错
          ElMessage.error(`加载菜单列表失败: ${error.message}`)
        }
      }
    }

    // 父菜单选项
    const parentMenuOptions = computed(() => {
      return menus.value.filter(menu => !menu.parentId)
    })

    // 处理分页大小变化
    const handleSizeChange = (size) => {
      pageSize.value = size
      loadMenus()
    }

    // 处理当前页码变化
    const handleCurrentChange = (current) => {
      currentPage.value = current
      loadMenus()
    }

    // 处理新增菜单
    const handleCreateMenu = () => {
      dialogTitle.value = '新增菜单'
      form.value = {
        name: '',
        path: '',
        icon: 'List',
        order: 1,
        parentId: ''
      }
      dialogVisible.value = true
    }

    // 处理新增子菜单
    const handleCreateSubMenu = (parentMenu) => {
      dialogTitle.value = '新增子菜单'
      form.value = {
        name: '',
        path: '',
        icon: 'List',
        order: 1,
        parentId: parentMenu.id
      }
      dialogVisible.value = true
    }

    // 处理编辑菜单
    const handleEditMenu = (menu) => {
      dialogTitle.value = '编辑菜单'
      form.value = { ...menu }
      dialogVisible.value = true
    }

    // 处理保存菜单
    const handleSaveMenu = async () => {
      try {
        const menuData = {
          name: form.value.name,
          path: form.value.path,
          icon: form.value.icon,
          order: form.value.order,
          parentId: form.value.parentId || null
        }

        const api = await import('@/api')
        if (form.value.id) {
          // 更新菜单
          await api.default.menus.updateMenu(form.value.id, menuData)
          ElMessage.success('更新菜单成功')
        } else {
          // 新增菜单
          await api.default.menus.createMenu(menuData)
          ElMessage.success('新增菜单成功')
        }
        dialogVisible.value = false
        loadMenus()
      } catch (error) {
        console.error('保存菜单失败:', error)
        ElMessage.error('保存菜单失败')
      }
    }

    // 处理删除菜单
    const handleDeleteMenu = async (id) => {
      try {
        await ElMessageBox.confirm('确定要删除这个菜单吗？', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        })

        const api = await import('@/api')
        await api.default.menus.deleteMenu(id)
        ElMessage.success('删除菜单成功')
        loadMenus()
      } catch (error) {
        if (error !== 'cancel') {
          console.error('删除菜单失败:', error)
          ElMessage.error('删除菜单失败')
        }
      }
    }

    // 页面加载时初始化数据
    onMounted(() => {
      loadMenus()
    })

    return {
      menus,
      menuList,
      parentMenuOptions,
      dialogVisible,
      dialogTitle,
      form,
      filterText,
      currentPage,
      pageSize,
      total,
      handleSizeChange,
      handleCurrentChange,
      handleCreateMenu,
      handleCreateSubMenu,
      handleEditMenu,
      handleSaveMenu,
      handleDeleteMenu
    }
  }
}
</script>

<style scoped>
.menus-view {
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
</style>

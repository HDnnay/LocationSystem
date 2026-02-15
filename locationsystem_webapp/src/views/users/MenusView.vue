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
          <el-button size="small" type="info" @click="handleAssignPermissions(scope.row)">
            <el-icon><SetUp /></el-icon>
            分配权限
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
          <IconSelector v-model="form.icon" placeholder="选择菜单图标" />
        </el-form-item>
        <el-form-item label="排序">
          <el-input-number v-model="form.order" :min="1" :max="100" />
        </el-form-item>
        <el-form-item label="父菜单">
          <el-cascader
            v-model="cascaderValue"
            :options="menuTreeOptions"
            :props="cascaderProps"
            placeholder="选择父菜单（可选）"
            clearable
            style="width: 100%"
            @change="handleCascaderChange"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSaveMenu">保存</el-button>
        </span>
      </template>
    </el-dialog>

    <!-- 分配权限对话框 -->
    <el-dialog
      v-model="permissionDialogVisible"
      :title="`为菜单分配权限 - ${selectedMenu?.name || ''}`"
      width="600px"
    >
      <PermissionTree
        :permissionTree="permissionTree"
        :selectedPermissions="checkedPermissionIds"
        :loading="isLoadingPermissions"
        :error="permissionError"
        @permission-change="handlePermissionChange"
      />
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="permissionDialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSavePermissions">保存</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { Plus, Edit, Delete, SetUp } from '@element-plus/icons-vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import IconSelector from '@/components/IconSelector.vue'
import PermissionTree from '@/components/PermissionTree.vue'

export default {
  name: 'MenusView',
  components: {
    Plus,
    Edit,
    Delete,
    SetUp,
    IconSelector,
    PermissionTree
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
    const isLoading = ref(false)
    const menuTreeOptions = ref([])
    const cascaderValue = ref([])
    const cascaderProps = {
      children: 'children',
      label: 'label',
      value: 'id',
      checkStrictly: true
    }
    // 分配权限相关
    const permissionDialogVisible = ref(false)
    const selectedMenu = ref(null)
    const permissionTree = ref([])
    const checkedPermissionIds = ref([])
    const isLoadingPermissions = ref(false)
    const permissionError = ref(null)
    // 加载所有菜单（用于构建完整的菜单树）
    const loadAllMenus = async () => {
      try {
        const api = await import('@/api')
        // 调用后端新提供的获取菜单树形结构的API
        const response = await api.default.menus.getMenuTree()
        console.log('获取菜单树形结构响应:', response)
        return response || []
      } catch (error) {
        console.error('加载所有菜单失败:', error)
        return []
      }
    }

    // 加载菜单列表（带分页）
    const loadMenus = async () => {
      if (isLoading.value) return

      try {
        isLoading.value = true
        console.log("进来一次");
        // 从后端获取菜单列表（带分页）
        const api = await import('@/api')
        const response = await api.default.menus.getAllMenus(currentPage.value, pageSize.value)
        console.log(response)
        // 直接使用response作为菜单列表，因为后端直接返回了菜单列表
        menus.value = response.data
        total.value = response.total
        // 直接使用后端返回的分页数据
        menuList.value = response.data
        // 加载所有菜单并构建树形结构的菜单选项
        const allMenus = await loadAllMenus()
        menuTreeOptions.value = buildMenuTreeOptions(allMenus)
      } catch (error) {
        console.error('加载菜单列表失败:', error)

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
      } finally {
        isLoading.value = false
      }
    }

    // 父菜单选项（用于旧的select组件，暂时保留）
    const parentMenuOptions = computed(() => {
      return menus.value.filter(menu => !menu.parentId)
    })

    // 构建树形结构的菜单选项
    const buildMenuTreeOptions = (menuList) => {
      console.log('构建菜单树选项，输入数据:', menuList)
      // 检查menuList是否已经是树形结构（从后端API获取的）
      if (menuList && menuList.length > 0 && (menuList[0].hasOwnProperty('ChildMenus') || menuList[0].hasOwnProperty('childMenus'))) {
        // 如果是树形结构，直接转换为el-cascader需要的格式
        console.log('输入数据是树形结构，转换为cascader选项')
        const result = convertTreeToCascaderOptions(menuList)
        console.log('转换后的cascader选项:', result)
        return result
      }

      // 否则，按照原来的逻辑构建树形结构
      console.log('输入数据是扁平结构，构建树形结构')
      const menuMap = new Map()
      const rootMenus = []

      // 首先创建所有菜单节点的映射
      menuList.forEach(menu => {
        menuMap.set(menu.id, {
          id: menu.id,
          label: menu.name,
          path: menu.path,
          icon: menu.icon,
          disabled: false,
          children: []
        })
      })

      // 然后构建树形结构
      menuList.forEach(menu => {
        const menuNode = menuMap.get(menu.id)
        if (menu.parentId) {
          // 如果有父菜单，添加到父菜单的子节点中
          const parentNode = menuMap.get(menu.parentId)
          if (parentNode) {
            parentNode.children.push(menuNode)
          } else {
            // 如果父菜单不存在，作为根节点处理
            rootMenus.push(menuNode)
          }
        } else {
          // 没有父菜单的作为根节点
          rootMenus.push(menuNode)
        }
      })

      // 对根菜单按order排序
      rootMenus.sort((a, b) => {
        // 尝试从menuList中找到对应的原始菜单，获取order值
        const menuA = menuList.find(m => m.id === a.id)
        const menuB = menuList.find(m => m.id === b.id)
        const orderA = menuA ? menuA.order : 0
        const orderB = menuB ? menuB.order : 0
        return orderA - orderB
      })

      // 递归对所有子菜单按order排序
      const sortChildren = (nodes) => {
        nodes.forEach(node => {
          if (node.children && node.children.length > 0) {
            // 对子菜单按order排序
            node.children.sort((a, b) => {
              const menuA = menuList.find(m => m.id === a.id)
              const menuB = menuList.find(m => m.id === b.id)
              const orderA = menuA ? menuA.order : 0
              const orderB = menuB ? menuB.order : 0
              return orderA - orderB
            })
            // 递归排序子菜单的子菜单
            sortChildren(node.children)
          }
        })
      }

      // 对所有子菜单排序
      sortChildren(rootMenus)

      return rootMenus
    }

    // 将后端返回的树形结构转换为el-cascader需要的格式
    const convertTreeToCascaderOptions = (menuTree) => {
      return menuTree.map(menu => {
        const option = {
          id: menu.id,
          label: menu.name,
          path: menu.path,
          icon: menu.icon,
          disabled: false,
          children: []
        }

        // 检查menu是否有ChildMenus或childMenus属性
        const childMenus = menu.ChildMenus || menu.childMenus
        if (childMenus && childMenus.length > 0) {
          option.children = convertTreeToCascaderOptions(childMenus)
        }

        return option
      })
    }

    // 处理级联选择器变化
    const handleCascaderChange = (value) => {
      if (value && value.length > 0) {
        form.value.parentId = value[value.length - 1]
      } else {
        form.value.parentId = null
      }
    }

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
        parentId: null
      }
      cascaderValue.value = []
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
      cascaderValue.value = [parentMenu.id]
      dialogVisible.value = true
    }

    // 处理编辑菜单
    const handleEditMenu = (menu) => {
      dialogTitle.value = '编辑菜单'
      form.value = { ...menu }
      cascaderValue.value = menu.parentId ? [menu.parentId] : []
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

    // 加载权限树
    const loadPermissions = async () => {
      try {
        isLoadingPermissions.value = true
        permissionError.value = null
        const api = await import('@/api')
        const response = await api.default.permissions.getPermissionTree()
        permissionTree.value = response
      } catch (error) {
        console.error('加载权限树失败:', error)
        permissionError.value = error.message
        ElMessage.error('加载权限树失败')
      } finally {
        isLoadingPermissions.value = false
      }
    }

    // 处理权限变化
    const handlePermissionChange = (selectedIds) => {
      checkedPermissionIds.value = selectedIds
    }

    // 处理分配权限
    const handleAssignPermissions = async (menu) => {
      selectedMenu.value = menu
      checkedPermissionIds.value = []
      await loadPermissions()
      permissionDialogVisible.value = true
    }

    // 处理保存权限
    const handleSavePermissions = async () => {
      try {
        const api = await import('@/api')
        await api.default.menus.assignPermissionsToMenu(selectedMenu.value.id, checkedPermissionIds.value)
        ElMessage.success('分配权限成功')
        permissionDialogVisible.value = false
      } catch (error) {
        console.error('分配权限失败:', error)
        ElMessage.error('分配权限失败')
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
      menuTreeOptions,
      cascaderValue,
      cascaderProps,
      dialogVisible,
      dialogTitle,
      form,
      filterText,
      currentPage,
      pageSize,
      total,
      permissionDialogVisible,
      selectedMenu,
      permissionTree,
      checkedPermissionIds,
      isLoadingPermissions,
      permissionError,
      handleSizeChange,
      handleCurrentChange,
      handleCascaderChange,
      handleCreateMenu,
      handleCreateSubMenu,
      handleEditMenu,
      handleSaveMenu,
      handleDeleteMenu,
      handleAssignPermissions,
      handleSavePermissions,
      handlePermissionChange
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

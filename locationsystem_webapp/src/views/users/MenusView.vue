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
    <el-tree
      :data="menuTree"
      node-key="id"
      :default-expand-all="true"
      :expand-on-click-node="false"
      :filter-node-method="filterNode"
    >
      <template #default="{ node, data }">
        <div class="tree-node">
          <span>{{ data.name }}</span>
          <span class="node-actions">
            <el-button size="small" @click="handleEditMenu(data)">
              <el-icon><Edit /></el-icon>
              编辑
            </el-button>
            <el-button size="small" type="danger" @click="handleDeleteMenu(data.id)">
              <el-icon><Delete /></el-icon>
              删除
            </el-button>
            <el-button size="small" type="warning" @click="handleCreateSubMenu(data)">
              <el-icon><Plus /></el-icon>
              子菜单
            </el-button>
          </span>
        </div>
      </template>
    </el-tree>
    
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

export default {
  name: 'MenusView',
  components: {
    Plus,
    Edit,
    Delete
  },
  setup() {
    const menus = ref([])
    const dialogVisible = ref(false)
    const dialogTitle = ref('新增菜单')
    const form = ref({})
    const filterText = ref('')
    
    // 加载菜单列表
    const loadMenus = async () => {
      try {
        const response = await import('@/api').then(m => m.default.menus.getAllMenus())
        menus.value = response.data
      } catch (error) {
        console.error('加载菜单列表失败:', error)
        ElMessage.error('加载菜单列表失败')
      }
    }
    
    // 构建菜单树
    const menuTree = computed(() => {
      // 过滤出顶级菜单
      return menus.value.filter(menu => !menu.parentId)
    })
    
    // 父菜单选项
    const parentMenuOptions = computed(() => {
      return menus.value.filter(menu => !menu.parentId)
    })
    
    // 过滤节点
    const filterNode = (value, data) => {
      if (!value) return true
      return data.name.toLowerCase().includes(value.toLowerCase())
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
        
        if (form.value.id) {
          // 更新菜单
          await import('@/api').then(m => m.default.menus.updateMenu(form.value.id, menuData))
          ElMessage.success('更新菜单成功')
        } else {
          // 新增菜单
          await import('@/api').then(m => m.default.menus.createMenu(menuData))
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
        
        await import('@/api').then(m => m.default.menus.deleteMenu(id))
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
      menuTree,
      parentMenuOptions,
      dialogVisible,
      dialogTitle,
      form,
      filterText,
      filterNode,
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

.tree-node {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.node-actions {
  display: flex;
  gap: 5px;
}

.node-actions .el-button {
  margin: 0;
}
</style>

<template>
  <div class="icon-selector">
    <!-- 图标输入框 -->
    <el-input
      v-model="iconValue"
      :placeholder="placeholder"
      @focus="showIconPanel = true"
    >
      <template #append>
        <el-button @click="showIconPanel = !showIconPanel">
          <component :is="getIconComponent(iconValue) || 'el-icon'">
            <el-icon><List /></el-icon>
          </component>
        </el-button>
      </template>
    </el-input>

    <!-- 图标选择面板 -->
    <el-popover
      v-model:visible="showIconPanel"
      placement="bottom"
      :width="400"
      trigger="manual"
    >
      <template #reference>
        <!-- 这个元素不会显示，只是作为popover的参考点 -->
        <span style="display: none;"></span>
      </template>

      <div class="icon-panel">
        <!-- 搜索框 -->
        <el-input
          v-model="searchText"
          placeholder="搜索图标名称"
          prefix-icon="el-icon-Search"
        />

        <!-- 图标列表 -->
        <div class="icon-list">
          <div
            v-for="icon in filteredIcons"
            :key="icon"
            class="icon-item"
            @click="selectIcon(icon)"
          >
            <component :is="getIconComponent(icon)">
              <el-icon><component :is="icon" /></el-icon>
            </component>
            <span class="icon-name">{{ icon }}</span>
          </div>
        </div>

        <!-- 分页 -->
        <el-pagination
          v-model:current-page="currentPage"
          :page-size="pageSize"
          layout="prev, pager, next"
          :total="filteredIcons.length"
          @current-change="handleCurrentChange"
        />
      </div>
    </el-popover>
  </div>
</template>

<script>
import { ref, computed, watch } from 'vue'
import { List, Search } from '@element-plus/icons-vue'
import * as Icons from '@element-plus/icons-vue'

export default {
  name: 'IconSelector',
  components: {
    List,
    Search
  },
  props: {
    modelValue: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: '选择图标'
    }
  },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    // 响应式数据
    const iconValue = ref(props.modelValue)
    const showIconPanel = ref(false)
    const searchText = ref('')
    const currentPage = ref(1)
    const pageSize = ref(30)

    // 监听props变化，更新本地值
    watch(() => props.modelValue, (newValue) => {
      iconValue.value = newValue
    })

    // 监听本地值变化，触发更新事件
    watch(iconValue, (newValue) => {
      emit('update:modelValue', newValue)
    })

    // 所有图标列表
    const allIcons = computed(() => {
      return Object.keys(Icons)
    })

    // 过滤后的图标列表
    const filteredIcons = computed(() => {
      if (!searchText.value) {
        return allIcons.value
      }
      return allIcons.value.filter(icon => {
        return icon.toLowerCase().includes(searchText.value.toLowerCase())
      })
    })

    // 分页后的图标列表
    const paginatedIcons = computed(() => {
      const start = (currentPage.value - 1) * pageSize.value
      const end = start + pageSize.value
      return filteredIcons.value.slice(start, end)
    })

    // 选择图标
    const selectIcon = (icon) => {
      iconValue.value = icon
      showIconPanel.value = false
    }

    // 处理分页变化
    const handleCurrentChange = (page) => {
      currentPage.value = page
    }

    // 获取图标组件
    const getIconComponent = (iconName) => {
      if (!iconName) return null
      return Icons[iconName]
    }

    return {
      iconValue,
      showIconPanel,
      searchText,
      currentPage,
      pageSize,
      filteredIcons,
      paginatedIcons,
      selectIcon,
      handleCurrentChange,
      getIconComponent
    }
  }
}
</script>

<style scoped>
.icon-panel {
  padding: 10px;
}

.icon-list {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 10px;
  margin: 15px 0;
  max-height: 300px;
  overflow-y: auto;
}

.icon-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 10px;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.icon-item:hover {
  background-color: #f5f7fa;
}

.icon-item .icon {
  font-size: 24px;
  margin-bottom: 5px;
}

.icon-item .icon-name {
  font-size: 12px;
  color: #909399;
  text-align: center;
  word-break: break-all;
}
</style>
